using System.Windows;
using WpfApp1.Pages;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public user_ CurrentUser { get; set; }

        public MainWindow(user_ user)
        {
            InitializeComponent();
            CurrentUser = user;

            // Настройка интерфейса в зависимости от роли
            SetupSidebar();

            if (user.RoleID == 3) // Администратор
            {
                BtnAdmin.Visibility = Visibility.Visible;
            }
            else if (user.RoleID == 2) // Автор
            {
                BtnAuthor.Visibility = Visibility.Visible;
            }
            else // Обычный читатель
            {
                BtnAdmin.Visibility = Visibility.Collapsed;
                BtnAuthor.Visibility = Visibility.Collapsed;
            }

            // По умолчанию открываем каталог
            MainFrame.Navigate(new CatalogPage(CurrentUser));
        }

        private void SetupSidebar()
        {
            if (CurrentUser == null) return;
            // Роль 2 = Автор
            if (CurrentUser.RoleID == 2)
            {
                BtnAuthor.Visibility = Visibility.Visible;
            }
            // Роль 3 = Администратор
            else if (CurrentUser.RoleID == 3)
            {
                BtnAdmin.Visibility = Visibility.Visible;
            }
        }

        private void BtnCatalog_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CatalogPage(CurrentUser));
        }

        private void BtnLists_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BookListPage(CurrentUser));
        }

        private void BtnAuthor_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AuthorPage(CurrentUser));
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AdminPage(CurrentUser));
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProfilePage(CurrentUser));
        }
    }
}