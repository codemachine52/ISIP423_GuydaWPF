using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Anastasia423WPF.Windows;

namespace Anastasia423WPF.Pages
{
    public partial class AdminPage : Page
    {
        private User _admin;
        // Список ролей для привязки к ComboBox в таблице
        public List<Role> AllRoles { get; set; }

        public AdminPage(User user)
        {
            InitializeComponent();
            _admin = user;

            // Менеджер (3) видит только товары, Админ (4) — всё
            if (_admin.RoleID != 4)
            {
                UsersTab.Visibility = Visibility.Collapsed;
            }

            LoadData();
        }

        private void LoadData()
        {
            AllRoles = Core.Context.Role.ToList();
            ProductsGrid.ItemsSource = Core.Context.Product.ToList();
            UsersGrid.ItemsSource = Core.Context.User.ToList();

            // Устанавливаем DataContext, чтобы DataGrid видел список AllRoles
            this.DataContext = this;
        }

        // Событие смены роли пользователя
        private void RoleCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;
            // ФИКС: Если выпадающий список закрыт, значит изменение программное — игнорируем его
            if (!combo.IsDropDownOpen) return;

            var selectedUser = combo.DataContext as User;
            var newRole = combo.SelectedItem as Role;

            if (selectedUser != null && newRole != null)
            {
                // Если админ пытается сменить роль САМ СЕБЕ
                if (selectedUser.ID == _admin.ID && newRole.ID != 4)
                {
                    MessageBox.Show("Нельзя лишать прав самого себя!");
                    // Отменяем визуально
                    combo.SelectedValue = 4;
                    return;
                }

                selectedUser.RoleID = newRole.ID;
                Core.Context.SaveChanges();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Catalog(_admin));
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            new AddEditProductWindow(null).ShowDialog();
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selected)
                new AddEditProductWindow(selected).ShowDialog();
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selected)
            {
                var result = MessageBox.Show($"Удалить {selected.Name}?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Core.Context.Product.Remove(selected);
                    Core.Context.SaveChanges();
                    LoadData();
                }
            }
        }
    }
}