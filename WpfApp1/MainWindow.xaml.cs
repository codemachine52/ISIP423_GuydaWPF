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
using WpfApp1.Pages;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var car = new Car();
            NavigationData.CurrentData = car;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Page currentPage = MainFrame.Content as Page;
            if (MainFrame.NavigationService.CanGoBack && !(currentPage is Results)) MainFrame.NavigationService.GoBack();
            if (currentPage is Results)
            {
                dataLoss.Visibility = Visibility.Visible;
            }
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.NavigationService.CanGoBack) MainFrame.NavigationService.GoBack();
            dataLoss.Visibility = Visibility.Hidden;
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            dataLoss.Visibility = Visibility.Hidden;
        }
    }

    public static class NavigationData
    {
        public static object CurrentData
        {
            get; set;
        }
    }

}
