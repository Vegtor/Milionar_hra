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
    /// Interakční logika pro Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        private MainWindow mainWindow;
        public Menu(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void startGameButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangePages(1);
        }

        private void closeGameButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
            System.Environment.Exit(0);
        }
    }
}
