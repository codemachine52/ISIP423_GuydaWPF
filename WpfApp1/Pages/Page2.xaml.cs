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
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        Client Us;

        public Page2()
        {
            InitializeComponent();
        }

        public Page2(Client user) : this()
        {
            InitializeComponent();
            Us = user;
            LoginEnter.Text = user.Email;
            PasswordEnter.Password = user.Password;
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page3());
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (AuthUser(LoginEnter.Text, PasswordEnter.Password))
            {
                NavigationService.Navigate(new Page1(Us));
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Пользователь не найден в Базе данных! Желаете зарегистрироваться?", "Ошибка входа", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                {
                    NavigationService.Navigate(new Page3());
                }
            }
        }

        public bool AuthUser(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return false;
            }
            else
            {
                var cl = Core.Context.Client.Where(c => (c.Email == login) && (c.Password == password)).FirstOrDefault();
                if (cl != null)
                {
                    Us = cl;
                    MessageBox.Show("Вы успешно авторизовались!");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1());
        }
    }
}
