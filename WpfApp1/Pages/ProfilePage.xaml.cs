using System.Linq;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class ProfilePage : Page
    {
        public ProfilePage(user_ user)
        {
            InitializeComponent();
            TBlockFIO.Text = $"ФИО: {user.Name}";
            TBlockEmail.Text = $"Email: {user.Email}";
            TBlockRole.Text = $"Статус: {user.role.Name}"; // Используем связь с таблицей Role

            // Загружаем список прочитанных книг из таблицы readList
            var myBooks = Core.Context.readList.Where(r => r.UserID == user.ID).Select(r => r.book.Name).ToList();
            LBoxMyBooks.ItemsSource = myBooks;
        }
    }
}