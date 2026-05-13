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
    /// Логика взаимодействия для AuthorPage.xaml
    /// </summary>
    public partial class AuthorPage : Page
    {
        private user_ _currentUser;

        public AuthorPage(user_ user)
        {
            InitializeComponent();
            _currentUser = user;
            UpdateData();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            try
            {
                Core.Context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                LBoxAuthorBooks.ItemsSource = Core.Context.book.Where(b => b.AuthorID == _currentUser.ID).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении данных: " + ex.Message);
            }
        }

        private void LBoxAuthorBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Получаем выбранную книгу
            var selectedBook = LBoxAuthorBooks.SelectedItem as book;
            if (selectedBook != null)
            {
                // Открываем окно для редактирования, передавая объект книги
                EditBookWindow editWin = new EditBookWindow(selectedBook, _currentUser);
                if (editWin.ShowDialog() == true)
                {
                    UpdateData(); // Обновляем список после сохранения
                }
            }
        }

        private void BtnAddBook_Click(object sender, RoutedEventArgs e)
        {
            // Открываем то же окно, но без параметров (для новой книги)
            EditBookWindow addWin = new EditBookWindow(null, _currentUser);
            if (addWin.ShowDialog() == true)
            {
                UpdateData();
            }
        }
        private void BtnAppeal_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = (sender as Button).Tag as book;
            if (selectedBook == null) return;

            string reason = $"Ваше произведение '{selectedBook.Name}' было заморожено модератором.";

            // Вызываем универсальное окно апелляции, передаем юзера, причину и ID книги
            FreezeAppealWindow appealWin = new FreezeAppealWindow(Core.CurrentUser, reason, selectedBook.ID);
            appealWin.ShowDialog();
        }
    }
}
