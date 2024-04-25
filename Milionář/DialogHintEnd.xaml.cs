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
using System.Windows.Shapes;

namespace Milionář
{
    /// <summary>
    /// Interakční logika pro DialogHintEnd.xaml
    /// </summary>
    public partial class DialogHintEnd : Window
    {
        MainWindow mainWindow;

        public DialogHintEnd(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        public void changeTexts(string header, string text)
        {
            this.headerDialog.Content = header;
            this.textDialog.Text = text;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.mainWindow.IsEnabled = true;
        }
    }
}
