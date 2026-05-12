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

            // 1. Загружаем все жанры из базы
            var allGenres = Core.Context.ganre.ToList();

            if (selectedBook != null)
            {
                _currentBook = selectedBook;
                _isEdit = true;
                TBoxName.Text = _currentBook.Name;
                TBoxDescription.Text = _currentBook.Description;
                TBoxPicture.Text = _currentBook.Picture;
                TBoxContent.Text = _currentBook.Text;

                // 2. Если редактируем, отмечаем те жанры, которые уже есть у книги
                var currentGenreIds = _currentBook.BookGanre.Select(bg => bg.GanreID).ToList();
                foreach (var g in allGenres)
                {
                    if (currentGenreIds.Contains(g.ID)) g.IsSelected = true;
                }
            }
            else
            {
                _currentBook = new book();
            }

            LBoxGenres.ItemsSource = allGenres;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBoxName.Text)) { MessageBox.Show("Укажите название!"); return; }

            _currentBook.Name = TBoxName.Text;
            _currentBook.Description = TBoxDescription.Text;
            _currentBook.Picture = TBoxPicture.Text;
            _currentBook.Text = TBoxContent.Text;

            if (!_isEdit)
            {
                _currentBook.AuthorID = _currentUser.ID;
                _currentBook.Rating = 0;
                _currentBook.IsFreeze = false; // По умолчанию книга активна
                _currentBook.BookPath = null;
                Core.Context.book.Add(_currentBook);
            }
            else
            {
                // Если редактируем — удаляем старые связи с жанрами, чтобы записать новые
                var oldGenres = Core.Context.BookGanre.Where(bg => bg.BookID == _currentBook.ID);
                Core.Context.BookGanre.RemoveRange(oldGenres);
            }

            // 3. Сохраняем выбранные жанры в таблицу-посредник
            foreach (ganre g in LBoxGenres.ItemsSource)
            {
                if (g.IsSelected)
                {
                    Core.Context.BookGanre.Add(new BookGanre
                    {
                        book = _currentBook,
                        GanreID = g.ID
                    });
                }
            }

            try
            {
                Core.Context.SaveChanges();
                MessageBox.Show("Книга успешно сохранена!");
                this.DialogResult = true;
            }
            catch (Exception ex) { MessageBox.Show("Ошибка сохранения: " + ex.Message); }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
