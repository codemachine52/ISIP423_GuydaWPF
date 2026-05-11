using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Windows
{
    public partial class AddReviewWindow : Window
    {
        private int _bookId;
        private int _userId;

        public AddReviewWindow(int bookId, int userId)
        {
            InitializeComponent();
            _bookId = bookId;
            _userId = userId;
        }

        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBoxReviewText.Text))
            {
                MessageBox.Show("Напишите текст отзыва!");
                return;
            }

            try
            {
                var newReview = new review
                {
                    BookID = _bookId,
                    UserID = _userId,
                    Description = TBoxReviewText.Text,
                    Mark = int.Parse((ComboRating.SelectedItem as ComboBoxItem).Content.ToString()),
                    Date = DateTime.Now
                };

                Core.Context.review.Add(newReview);
                Core.Context.SaveChanges();

                MessageBox.Show("Отзыв успешно добавлен!");
                this.DialogResult = true; // Закрывает окно и сообщает об успехе
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}