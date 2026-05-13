using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Windows;

namespace WpfApp1.Pages
{
    public partial class ProfilePage : Page
    {
        user_ currentUser;
        public ProfilePage(user_ user)
        {
            InitializeComponent();
            currentUser = user;
            TBlockFIO.Text = $"ФИО: {user.Name}";
            TBlockEmail.Text = $"Email: {user.Email}";
            TBlockRole.Text = $"Статус: {user.role.Name}";
            var myReviews = Core.Context.review
                .Where(r => r.UserID == user.ID)
                .ToList() // Сначала выгружаем в память для корректной работы
                .Select(r => new {
                    BookName = r.book.Name,
                    BookID = r.BookID,
                    Rating = r.Mark,
                    Description = r.Description
                })
                .ToList();

            LBoxMyReviews.ItemsSource = myReviews;

            if (user.RoleID != 1)
            {
                RequestButton.Visibility = Visibility.Collapsed;
            }
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

        private void RequestButton_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно как модальное
            var reviewWin = new RoleRequestWindow(currentUser.ID);
            reviewWin.Owner = Window.GetWindow(this); // Чтобы окно было по центру ReadPage

            if (reviewWin.ShowDialog() == true)
            {

            }
        }

        private void LBoxMyReviews_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedReview = LBoxMyReviews.SelectedItem;
            if (selectedReview == null) { return; }
            dynamic data = selectedReview;
            int? bID = data.BookID;

            if (bID != null)
            {
                var book = Core.Context.book.FirstOrDefault(b => b.ID == bID);
                if (book != null)
                {
                    NavigationService.Navigate(new BookDetailsPage(book, currentUser));
                }
            }
        }
    }
}