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
    /// Логика взаимодействия для BookDetailsPage.xaml
    /// </summary>
    public partial class BookDetailsPage : Page
    {
        private book _currentBook;
        private user_ _currentUser;

        public BookDetailsPage(book selectedBook, user_ user)
        {
            // 1. Сначала данные
            _currentBook = selectedBook;
            _currentUser = user;
            this.DataContext = _currentBook;

            // 2. Потом инициализация компонентов (Bindingи увидят готовый DataContext)
            InitializeComponent();

            // 3. Потом загрузка доп. данных
            LoadReviews();
        }

        private void LoadReviews()
        {
            // Загружаем отзывы, которые не заморожены (если не админ)
            var reviews = Core.Context.review.Where(r => r.BookID == _currentBook.ID);
            if (_currentUser.RoleID != 3)
                reviews = reviews.Where(r => r.IsFreeze != true);

            LBoxReviews.ItemsSource = reviews.ToList();
        }

        // Жалоба на книгу
        private void ReportBook_Click(object sender, RoutedEventArgs e)
        {
            // В твоей таблице report: id, userID, reviewID (может быть null)
            // Добавим запись о жалобе на книгу (reviewID оставляем null)
            Core.Context.report.Add(new report
            {
                userID = _currentUser.ID,
                BookID = _currentBook.ID,
                reviewID = null,
                AuthorID = null,
                reportDate = DateTime.Now
            });
            Core.Context.SaveChanges();
            MessageBox.Show("Жалоба на книгу отправлена модераторам");
        }

        // Жалоба на автора
        private void ReportAuthor_Click(object sender, RoutedEventArgs e)
        {
            var author = Core.Context.user_.Where(u => u.ID == _currentBook.AuthorID).FirstOrDefault();
            Core.Context.report.Add(new report
            {
                userID = _currentUser.ID,
                AuthorID = author.ID,
                reviewID = null,
                BookID = _currentBook.ID,
                reportDate = DateTime.Now
            });
            Core.Context.SaveChanges();
            MessageBox.Show($"Жалоба на автора {_currentBook.user_.Name} отправлена");
        }

        // Пожаловаться на отзыв
        private void ReportReview_Click(object sender, RoutedEventArgs e)
        {
            var rev = (sender as Button).Tag as review;
            Core.Context.report.Add(new report
            {
                userID = _currentUser.ID,
                reviewID = rev.ID,
                userWasReportedID = rev.UserID,
                BookID = null,
                AuthorID = null,
                reportDate = DateTime.Now
            });
            Core.Context.SaveChanges();
            MessageBox.Show("Жалоба на отзыв принята");
        }

        // Заморозка книги (только для админа)
        private void AdminFreeze_Click(object sender, RoutedEventArgs e)
        {
            _currentBook.IsFreeze = !(_currentBook.IsFreeze == true);
            Core.Context.SaveChanges();
            NavigationService.GoBack(); // Возвращаемся в каталог
        }

        // Заморозка отзыва (только для админа)
        private void AdminFreezeReview_Click(object sender, RoutedEventArgs e)
        {
            var rev = (sender as Button).Tag as review;
            rev.IsFreeze = !(rev.IsFreeze == true);
            Core.Context.SaveChanges();
            LoadReviews();
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReadPage(_currentBook.ID, _currentUser));
        }
    }
}
