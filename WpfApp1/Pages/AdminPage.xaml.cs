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

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage(user_ currentUser)
        {
            InitializeComponent();
        }
        private void UpdateGrid()
        {
            GridUsers.ItemsSource = Core.Context.user_.ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) => UpdateGrid();

        private void BtnToggleFreeze_Click(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button).Tag as user_;

            // Инвертируем статус заморозки
            user.IsFreeze = !user.IsFreeze;

            try
            {
                Core.Context.SaveChanges();
                MessageBox.Show($"Статус пользователя {user.Login} изменен.");
                UpdateGrid();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
