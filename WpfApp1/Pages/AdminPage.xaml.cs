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
        private user_ _currentAdmin;
        public List<role> AllRoles { get; set; }
        
        public AdminPage(user_ currentUser)
        {
            InitializeComponent();
            RefreshData();
            _currentAdmin = currentUser;
            AllRoles = Core.Context.role.ToList();
            this.DataContext = this;
        }
        private void RefreshData()
        {
            Core.Context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            DGridUsers.ItemsSource = Core.Context.user_.ToList();
            LBoxUnfreezeRequests.ItemsSource = Core.Context.requestUnFreeze.Include("user_").ToList();
            LBoxRoleRequests.ItemsSource = Core.Context.requestRole.Include("user_").ToList();
        }

        private void FreezeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            var selectedUser = cb.DataContext as user_;

            if (selectedUser.ID == _currentAdmin.ID)
            {
                MessageBox.Show("Вы не можете заморозить самого себя!");
                selectedUser.IsFreeze = false; 
                cb.IsChecked = false;          
                return;
            }

            Core.Context.SaveChanges();
        }
        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            // чтобы база не дергалась при первой загрузке страницы
            if (cb != null && cb.IsLoaded)
            {
                var selectedUser = cb.DataContext as user_;
                if (selectedUser == null) return;
                // Проверка на самого себя
                if (selectedUser.ID == _currentAdmin.ID)
                {
                    // Если админ пытается сменить себе роль
                    if (selectedUser.RoleID != 3)
                    {
                        MessageBox.Show("Вы не можете сменить роль самому себе!");
                        selectedUser.RoleID = 3;
                        cb.SelectedValue = 3;
                        return;
                    }
                }

                try
                {
                    // Явно говорим контексту, что объект изменен
                    Core.Context.Entry(selectedUser).State = System.Data.Entity.EntityState.Modified;
                    Core.Context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении роли: " + ex.Message);
                }
            }
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
