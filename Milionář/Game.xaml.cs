using Microsoft.VisualBasic;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Milionář
{
    /// <summary>
    /// Interakční logika pro Game.xaml
    /// </summary>
    /// 
    public partial class Game : Page
    {
        private QuestionService questionService;
        private MainWindow mainWindow;
        private DialogHintEnd dialog;

        private int level = 1;
        private int rightAnswer;
        private string[] answers = { "A", "B", "C", "D" };
        private Button[] buttons = new Button[4];
        private HintService hintService;
        private int category;
        private int[] activeAnswers;
        private int[] permAnswers;
        Label[] labels = new Label[16];

        //Timer
        private readonly object lockObj = new object();
        private int number = 60;
        private CancellationTokenSource tokenSource;
        private Task timerTask;

        public Game(MainWindow mainWindow)
        {
            InitializeComponent();
            GenerateLevels();
            this.mainWindow = mainWindow;
            buttons[0] = this.A;
            buttons[1] = this.B;
            buttons[2] = this.C;
            buttons[3] = this.D;
            double[] temp = { 0.8, 0.7, 0.6, 0.8, 0.7, 0.7, 0.7, 0.8 };
            hintService = new HintService(temp);
            dialog = new DialogHintEnd(this.mainWindow);
        }

        public Grid LevelGrid 
        { 
            get 
            {
                return this.levelGrid;
            }
        }

        private void GenerateLevels()
        {

            int[] constOfLevels = { 1, 2, 3, 5, 10, 20, 40, 80, 160, 320, 640, 1250, 2500, 5000, 7500, 10000 };
            for (int i = 0; i < 16; i++)
            {
                this.labels[i] = new Label();
                this.labels[i].Name = "level" + i;
                this.labels[i].Content = constOfLevels[i] * 1000 + " Kč";
                this.labels[i].FontSize = 22; 
                this.levelGrid.Children.Add(this.labels[i]);
                Grid.SetRow(this.labels[i], 15 - i);
            }
            labels[0].Foreground = new SolidColorBrush(Colors.Green);
        }

        private void changeButton(Button btn, string type)
        {
            Image tempImage = (Image)(VisualTreeHelper.GetChild(btn, 0));
            tempImage.Source = new BitmapImage(new Uri("/resource/" + tempImage.Name + type, UriKind.Relative));
        }

        private void resetButtons()
        {
            for (int i = 0; i < 4; i++)
            {
                Button temp = buttons[i];
                temp.IsEnabled = true;
                this.changeButton(temp, "_norm.png");
            }
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            if (((Button)sender).IsEnabled == true)
            {
                Button temp = ((Button)sender);
                this.changeButton(temp, "_ozn.png");
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (((Button)sender).IsEnabled == true)
            {
                Button temp = ((Button)sender);
                this.changeButton(temp, "_norm.png");
            }
        }

        private void AnswerClicked(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).IsEnabled == true)
            {
                tokenSource.Cancel();

                for (int i = 0; i < 4; i++)
                {
                    buttons[0].IsEnabled = false;
                }

                Button temp = buttons[rightAnswer];
                this.changeButton(temp, "_dobr.png");


                if (((Button)sender).Name == answers[rightAnswer])
                {
                    labels[level].Foreground = new SolidColorBrush(Colors.Green);
                    labels[level-1].Foreground = new SolidColorBrush(Colors.Gray);
                    this.level++;
                    if(level == 15)
                    {
                        dialog.changeTexts("Konec hry", "Odpověděl si všechny otázky, hra pro tebe končí vítězně. Výsledná výherní částka je tedy 1 000 000 Kč.");
                        endOfGame();
                    }
                    Task.Delay(1000).Wait();
                    resetButtons();
                    roundOfGame();
                }
                else
                {
                    temp = ((Button)sender);
                    this.changeButton(temp, "_spat.png");
                    Task.Delay(1000).Wait();
                    dialog.changeTexts("Konec hry","Odpověděl si nesprávně, hra pro tebe končí. Výsledná výherní částka je " + labels[level-1].Content);
                    endOfGame();
                }
            }
        }

        private void endOfGame()
        {
            this.questionService.closeConnection();
            tokenSource.Cancel();
            tokenSource.Dispose();
            dialog.ShowDialog();
            this.mainWindow.Close();
        }

        private void HintClicked(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).IsEnabled == true)
            {
                Button temp = ((Button)sender);
                Image tempImage = (Image)(VisualTreeHelper.GetChild(temp, 0));
                this.changeButton(temp, "_pouz.png");
                ((Button)sender).IsEnabled = false;

                switch (tempImage.Name)
                {
                    case "hintPeople":
                        int[] peoplePercents = hintService.getPeopleHint(category, activeAnswers.Length, rightAnswer);
                        if (activeAnswers.Length == 4)
                        {
                            dialog.changeTexts("Nápověda publika", "Procenta pro jednotlivé odpovědi jsou: pro A - " + peoplePercents[0] + ", pro B - " + peoplePercents[1] + ", pro C - " + peoplePercents[2] + " a pro D - " + peoplePercents[3]);
                        }
                        else
                        {
                            dialog.changeTexts("Nápověda publika", "Procenta pro jednotlivé odpovědi jsou: pro " + buttons[Array.IndexOf(permAnswers,activeAnswers[0])] + " - " + peoplePercents[0] + ", pro " + buttons[Array.IndexOf(permAnswers, activeAnswers[1])] + " - " + peoplePercents[1]);
                        }
                        this.mainWindow.IsEnabled = false;
                        tokenSource.Cancel();
                        dialog.ShowDialog();
                        dialog = new DialogHintEnd(this.mainWindow);
                        resetTimer();
                        break;
                    //case "hintPhone":
                    //    HintPhone();
                    //    break;
                    default:
                        int[] answers5050 = hintService.get5050Hint(activeAnswers, rightAnswer);
                        buttons[Array.IndexOf(permAnswers, answers5050[0])].IsEnabled = false;
                        buttons[Array.IndexOf(permAnswers, answers5050[1])].IsEnabled = false;
                        this.changeButton(buttons[Array.IndexOf(permAnswers, answers5050[0])], "_spat.png");
                        this.changeButton(buttons[Array.IndexOf(permAnswers, answers5050[1])], "_spat.png");

                        List<int> numberList = activeAnswers.ToList();
                        numberList.Remove(answers5050[0]);
                        numberList.Remove(answers5050[1]);
                        activeAnswers = numberList.ToArray();
                        tokenSource.Cancel();
                        resetTimer();
                        break;
                }

            }
        }

        private void startTimer(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                lock (lockObj)
                {
                    number--;
                    updateGuiSafe(number);
                }

                Thread.Sleep(1000);

                if (number == 0)
                {
                    Dispatcher.Invoke(() => dialog.changeTexts("Konec hry", "Vypršel ti čas, hra pro tebe končí. Výsledná výherní částka je: " + labels[level - 1].Content));
                    Dispatcher.Invoke(() => endOfGame());
                }
            }
        }

        private void updateGuiSafe(int value)
        {
            if (Dispatcher.CheckAccess())
            {
                this.timerTextBlock.Text = value.ToString();
            }
            else
            {
                Dispatcher.Invoke(() => updateGuiSafe(value));
            }
        }

        private void resetTimer()
        {
            this.number = 60;
            if(tokenSource != null)
            {
                tokenSource.Dispose();
            }
            tokenSource = new CancellationTokenSource();
            timerTask = Task.Run(() => startTimer(tokenSource.Token));
        }

        private void roundOfGame()
        {
            string[] temp = questionService.GetQuestion(level);
            this.questionTextBlock.Text = temp[0];
            int[] indexes = { 1, 2, 3, 4 };
            Random rnd = new Random();
            activeAnswers = indexes.OrderBy(x => rnd.Next()).ToArray();
            this.answerATextblock.Text = temp[activeAnswers[0]];
            this.answerBTextblock.Text = temp[activeAnswers[1]];
            this.answerCTextblock.Text = temp[activeAnswers[2]];
            this.answerDTextblock.Text = temp[activeAnswers[3]];
            this.permAnswers = activeAnswers;
            this.rightAnswer = Array.IndexOf(activeAnswers, 1);
            this.category = Int32.Parse(temp[5]);
            resetTimer();
        }

        public void startOfGame()
        {
            try
            {
                questionService = new QuestionService();
                this.roundOfGame();
            }
            catch (Exception)
            {
                //showdialog
                throw;
            }
        }

    }
}
