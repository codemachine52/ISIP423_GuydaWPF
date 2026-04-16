using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Anastasia423WPF.Pages
{
    public partial class ProfilePage : Page
    {
        private User _currentUser;

        public ProfilePage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            this.DataContext = _currentUser;
            LoadOrders();
        }

        private void LoadOrders()
        {
            // Загружаем заказы конкретного пользователя
            var orders = Core.Context.Order
                .Where(o => o.ClientID == _currentUser.ID)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            OrdersList.ItemsSource = orders;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Catalog(_currentUser));
        }
    }
}