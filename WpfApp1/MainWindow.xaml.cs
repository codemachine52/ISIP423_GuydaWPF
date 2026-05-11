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

            // По умолчанию открываем каталог
            MainFrame.Navigate(new CatalogPage(CurrentUser));
        }

        private void SetupSidebar()
        {
            if (CurrentUser == null) return;

            // Если аккаунт заморожен, показываем снежинку
            if (CurrentUser.IsFreeze == true)
            {
                BtnFreezeWarning.Visibility = Visibility.Visible;
            }

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

        private void BtnFreezeWarning_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FreezeWarningPage(CurrentUser));
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProfilePage(CurrentUser));
        }
    }
}