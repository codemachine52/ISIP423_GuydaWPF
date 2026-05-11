using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Windows;

namespace WpfApp1.Pages
{
    public partial class ProfilePage : Page
    {
        public ProfilePage(user_ user)
        {
            InitializeComponent();
            TBlockFIO.Text = $"ФИО: {user.Name}";
            TBlockEmail.Text = $"Email: {user.Email}";
            TBlockRole.Text = $"Статус: {user.role.Name}"; // Используем связь с таблицей Role

            // Загружаем список прочитанных книг из таблицы readList
            var myBooks = Core.Context.readList.Where(r => r.UserID == user.ID).Select(r => r.book.Name).ToList();
            LBoxMyBooks.ItemsSource = myBooks;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            //Создаем и показываем окно авторизации
            AuthWindow auth = new AuthWindow();
            auth.Show();

            //Находим родительское окно (MainWindow), в котором находится текущая страница
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
    }
}