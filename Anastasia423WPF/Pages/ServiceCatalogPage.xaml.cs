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

namespace Anastasia423WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServiceCatalogPage.xaml
    /// </summary>
    public partial class ServiceCatalogPage : Page
    {
        public ServiceCatalogPage()
        {
            InitializeComponent();
        }

        // Код в ServiceCatalogPage.xaml.cs
        private void Book_Click(object sender, RoutedEventArgs e)
        {
            // Получаем услугу, на которую нажал пользователь
            var selectedService = (sender as Button).DataContext as Service;

            // Переходим на страницу бронирования, передавая саму услугу и текущего юзера
            NavigationService.Navigate(new ServiceBookingPage(selectedService, _currentUser));
        }
    }
}
