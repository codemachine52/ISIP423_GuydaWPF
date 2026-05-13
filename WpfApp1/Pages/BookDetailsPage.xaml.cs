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
using WpfApp1.Windows;

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
            _currentBook = selectedBook;
            _currentUser = user;
            this.DataContext = _currentBook;

            InitializeComponent();

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
        private void ReviewBut_Click(object sender, RoutedEventArgs e)
        {
            var reviewWin = new AddReviewWindow(_currentBook.ID, _currentUser.ID);
            reviewWin.Owner = Window.GetWindow(this);
            if (reviewWin.ShowDialog() == true)
            {

            }
        }
        private void ComboStatus_Loaded(object sender, RoutedEventArgs e)
        {
            var combo = sender as ComboBox;
            combo.ItemsSource = Core.Context.readStatus.ToList();

            // Подсвечиваем текущий статус книги для пользователя, если он есть
            int bookId = (int)combo.Tag;
            var currentStatus = Core.Context.readList
                .FirstOrDefault(r => r.BookID == bookId && r.UserID == _currentUser.ID);

            if (currentStatus != null)
                combo.SelectedValue = currentStatus.ReadStatusID;
        }

        private void ComboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;
            if (combo.SelectedValue == null) return;

            int bookId = (int)combo.Tag;
            int selectedStatusId = (int)combo.SelectedValue; // Получаем ID из ComboBox

            // Ищем запись в списках текущего пользователя
            var record = Core.Context.readList.FirstOrDefault(r => r.BookID == bookId && r.UserID == _currentUser.ID);

            if (record != null)
            {
                record.ReadStatusID = selectedStatusId;
            }
            else
            {
                // Создаем новую запись, если её не было
                Core.Context.readList.Add(new readList
                {
                    UserID = _currentUser.ID,
                    BookID = bookId,
                    ReadStatusID = selectedStatusId
                });
            }

            Core.Context.SaveChanges();
        }
    }
}
//заявку на автора из профиля
//вместо моя библиотека мои отзывы сделать
//список замороженных книг как пользователей выводить у админа
//автообновление рейтинга книги и отображение отзыва