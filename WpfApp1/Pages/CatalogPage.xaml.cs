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
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        private user_ _currentUser { get; set; }
        public CatalogPage(user_ us)
        {
            _currentUser = us;
            InitializeComponent();
            var genres = Core.Context.ganre.ToList();
            genres.Insert(0, new ganre { Name = "Все жанры" });
            ComboGenre.ItemsSource = genres;
            ComboGenre.SelectedIndex = 0;
            ComboSort.SelectedIndex = 0;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) => UpdateData();

        private void FilterChanged(object sender, EventArgs e) => UpdateData();

        // Метод для обновления списка (с учетом поиска)
        private void UpdateData()
        {
            var list = Core.Context.book.ToList();

            // 1. Поиск
            if (!string.IsNullOrWhiteSpace(TBoxSearch.Text))
                list = list.Where(p => p.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            // 2. Фильтрация по жанру
            if (ComboGenre.SelectedIndex > 0)
            {
                var selectedGenre = ComboGenre.SelectedItem as ganre;
                list = list.Where(p => p.BookGanre.Any(g => g.GanreID == selectedGenre.ID)).ToList();
            }

            // 3. Сортировка
            switch (ComboSort.SelectedIndex)
            {
                case 1: list = list.OrderBy(p => p.Name).ToList(); break;
                case 2: list = list.OrderByDescending(p => p.Name).ToList(); break;
                case 3: list = list.OrderBy(p => p.Rating).ToList(); break;
                case 4: list = list.OrderByDescending(p => p.Rating).ToList(); break;
            }

            // 4. Скрытие замороженных книг для обычных пользователей
            if (Core.CurrentUser.RoleID != 3)
                list = list.Where(p => p.IsFreeze != true).ToList();

            LBoxBooks.ItemsSource = list;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            var book = (sender as Button).Tag as book;
            NavigationService.Navigate(new BookDetailsPage(book, _currentUser));
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
