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
        }

        // Загрузка данных при открытии страницы
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
            MessageBox.Show($"User ID: {_currentUser.ID}");
        }

        // Метод для обновления списка (с учетом поиска)
        private void UpdateData()
        {
            var currentBooks = Core.Context.book.ToList();

            // Фильтрация по поиску
            if (!string.IsNullOrWhiteSpace(TBoxSearch.Text))
            {
                currentBooks = currentBooks
                    .Where(b => b.Name.ToLower().Contains(TBoxSearch.Text.ToLower()))
                    .ToList();
            }

            LBoxBooks.ItemsSource = currentBooks;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            // Логика перехода к чтению конкретной книги
            var button = sender as Button;
            int bookId = (int)button.Tag;
            NavigationService.Navigate(new ReadPage(bookId, _currentUser));
        }
    }
}
