using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Menu menuPage;
        private Game gamePage;
        public MainWindow()
        {
            menuPage = new Menu(this);
            gamePage = new Game(this);

            InitializeComponent();
            this.mainFrame.Content = menuPage;
        }
        

        public void ChangePages(int page)
        {
            if(page == 0)
            {
                this.mainFrame.Content = menuPage;
            }
            else
            {
                this.mainFrame.Content = gamePage;
                gamePage.startOfGame();

            }
        }
        
    }
}
