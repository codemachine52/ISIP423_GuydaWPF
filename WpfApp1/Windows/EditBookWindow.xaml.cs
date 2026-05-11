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
using System.Windows.Shapes;

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditBookWindow.xaml
    /// </summary>
    public partial class EditBookWindow : Window
    {
        private book _currentBook;
        private bool _isEdit = false;
        private user_ _currentUser;

        public EditBookWindow(book selectedBook, user_ us)
        {
            InitializeComponent();
            _currentUser = us;
            if (selectedBook != null)
            {
                _currentBook = selectedBook;
                _isEdit = true;
                // Заполняем поля данными для редактирования
                TBoxName.Text = _currentBook.Name;
                TBoxDescription.Text = _currentBook.Description;
                TBoxPicture.Text = _currentBook.Picture;
                TBoxContent.Text = _currentBook.Text; // Поле текста из БД
            }
            else
            {
                _currentBook = new book();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Базовая валидация
            if (string.IsNullOrWhiteSpace(TBoxName.Text)) { MessageBox.Show("Укажите название!"); return; }

            _currentBook.Name = TBoxName.Text;
            _currentBook.Description = TBoxDescription.Text;
            _currentBook.Picture = TBoxPicture.Text;
            _currentBook.Text = TBoxContent.Text;

            if (!_isEdit)
            {
                // Если новая книга, прописываем ID автора (текущего пользователя)
                // Допустим, мы сохранили текущего пользователя в статичном классе
                _currentBook.AuthorID = _currentUser.ID;
                _currentBook.Rating = 0; // Начальный рейтинг
                Core.Context.book.Add(_currentBook);
            }

            try
            {
                Core.Context.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                this.DialogResult = true;
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
