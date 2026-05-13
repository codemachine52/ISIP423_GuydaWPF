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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage(user_ currentUser)
        {
            InitializeComponent();
            RefreshData();
        }
            private void RefreshData()
        {
            // Отключаем кэширование, чтобы EF лез прямо в базу
            Core.Context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

            // Заполняем списки
            DGridUsers.ItemsSource = Core.Context.user_.ToList();
            LBoxUnfreezeRequests.ItemsSource = Core.Context.requestUnFreeze.ToList();
            LBoxRoleRequests.ItemsSource = Core.Context.requestRole.ToList();

            // Проверка для отладки: если после этого выскочит сообщение "0", значит в базе пусто
            // MessageBox.Show(Core.Context.user_.Count().ToString()); 
        }

        // РАЗМОРОЗКА
        private void BtnAcceptUnfreeze_Click(object sender, RoutedEventArgs e)
        {
            var req = (sender as Button).Tag as requestUnFreeze;
            if (req.bookID != null) req.book.IsFreeze = false; // Размораживаем книгу
            else req.user_.IsFreeze = false; // Или пользователя

            Core.Context.requestUnFreeze.Remove(req); // Удаляем заявку
            Core.Context.SaveChanges();
            RefreshData();
        }

        // РОЛЬ АВТОРА
        private void BtnAcceptAuthor_Click(object sender, RoutedEventArgs e)
        {
            var req = (sender as Button).Tag as requestRole;
            req.user_.RoleID = 2; // Меняем роль на Автора (ID 2)

            Core.Context.requestRole.Remove(req);
            Core.Context.SaveChanges();
            RefreshData();
        }

        // СМЕНА ПАРОЛЯ
        private void BtnChangePass_Click(object sender, RoutedEventArgs e)
        {
            var u = (sender as Button).Tag as user_;
            u.Password = "123"; // сброс на стандартный пароль
            Core.Context.SaveChanges();
            MessageBox.Show($"Пароль для {u.Login} сброшен на '123'");
        }

        // Метод отклонения заявки на роль АВТОРА
        private void BtnDeclineAuthor_Click(object sender, RoutedEventArgs e)
        {
            var req = (sender as Button).Tag as requestRole;
            if (req != null)
            {
                Core.Context.requestRole.Remove(req);
                Core.Context.SaveChanges();
                RefreshData(); // Обновляем списки на странице
                MessageBox.Show("Заявка на роль автора отклонена");
            }
        }

        // Метод отклонения заявки на РАЗМОРОЗКУ
        private void BtnDeclineUnfreeze_Click(object sender, RoutedEventArgs e)
        {
            var req = (sender as Button).Tag as requestUnFreeze;
            if (req != null)
            {
                Core.Context.requestUnFreeze.Remove(req);
                Core.Context.SaveChanges();
                RefreshData(); // Обновляем списки на странице
                MessageBox.Show("Заявка на разморозку отклонена");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) => RefreshData();


        
        //private void BtnToggleFreeze_Click(object sender, RoutedEventArgs e)
        //{
        //    var user = (sender as Button).Tag as user_;

        //    // Инвертируем статус заморозки
        //    user.IsFreeze = !user.IsFreeze;

        //    try
        //    {
        //        Core.Context.SaveChanges();
        //        MessageBox.Show($"Статус пользователя {user.Login} изменен.");
        //        RefreshData();
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
    }
}
