using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Anastasia423WPF.Windows;

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

        private void OrdersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Берем выбранный заказ из списка в профиле
            if (OrdersList.SelectedItem is Order selectedOrder)
            {
                // Создаем окно, передаем в него заказ и показываем
                OrderDetailsWindow details = new OrderDetailsWindow(selectedOrder);
                details.Owner = Window.GetWindow(this); // Чтобы окно было привязано к главному
                details.ShowDialog(); // ShowDialog заблокирует профиль, пока окно не закроют
            }
        }
    }
}