using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Windows;

namespace WpfApp1.Pages
{
    public partial class ReadPage : Page
    {
        private book _currentBook;
        private user_ _currentUser;

        public ReadPage(int bookId, user_ user)
        {
            InitializeComponent();
            _currentUser = user;
            _currentBook = Core.Context.book.FirstOrDefault(b => b.ID == bookId);

            if (_currentBook != null)
            {
                TBlockBookName.Text = _currentBook.Name;
                // В приложении текст загружается из БД
                TBlockContent.Text = $"Вы начали чтение книги: {_currentBook.Name}. \n\nОписание: {_currentBook.Description}\n\nТЕКСТ: {_currentBook.Text}";
            }
            //MessageBox.Show($"User ID: {_currentUser.ID}");
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();

        private void OpenReviewWindow_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно как модальное
            var reviewWin = new AddReviewWindow(_currentBook.ID, _currentUser.ID);
            reviewWin.Owner = Window.GetWindow(this); // Чтобы окно было по центру ReadPage

            if (reviewWin.ShowDialog() == true)
            {
                // Можно обновить данные на странице, если это нужно
            }
        }
    }
}