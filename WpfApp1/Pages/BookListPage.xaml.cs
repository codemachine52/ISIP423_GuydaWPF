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
    /// Логика взаимодействия для BookListPage.xaml
    /// </summary>
    public partial class BookListPage : Page
    {
        private user_ _currentUser;
        public BookListPage(user_ us)
        {
            _currentUser = us;
            InitializeComponent();
            LBoxStatuses.ItemsSource = Core.Context.readStatus.ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void LBoxStatuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            var selectedStatus = LBoxStatuses.SelectedItem as readStatus;
            var query = Core.Context.readList.Where(r => r.UserID == _currentUser.ID);

            if (selectedStatus != null)
            {
                query = query.Where(r => r.ReadStatusID == selectedStatus.ID);
            }

            LBoxFilteredBooks.ItemsSource = query.ToList();
        }

        private void LBoxFilteredBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Получаем выбранный объект из списка (это тип readList)
            var selectedRecord = LBoxFilteredBooks.SelectedItem as readList;

            if (selectedRecord != null && selectedRecord.book != null)
            {
                // Переходим на страницу ReadPage, передавая ID книги и текущего пользователя
                // Убедись, что конструктор ReadPage принимает эти параметры
                NavigationService.Navigate(new BookDetailsPage(selectedRecord.book, _currentUser));
            }
        }
    }
}
