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
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Page
    {
        public User user { get; set; }
        public Catalog()
        {
            InitializeComponent();
        }

        public Catalog(User us): this()
        {
            user = us;
        }

        private void AuthUser_Click(object sender, RoutedEventArgs e)
        {
            if (user != null)
            {
                NavigationService.Navigate(new AuthPage());
            }
            else
            {
                MessageBox.Show("Страница аккаунта в разработке", "Переход невозможен", MessageBoxButton.OK, MessageBoxImage.Information);
                //NavigationService.Navigate(new AuthPage(user));
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }
    }
}
