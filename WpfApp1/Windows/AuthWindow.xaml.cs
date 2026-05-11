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
using WpfApp1.Windows;

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public user_ user { get; set; }

        public AuthWindow()
        {
            InitializeComponent();
        }

        private void Registr_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow regWin = new RegistrationWindow();
            regWin.Show();
            this.Close(); // Закрываем окно входа при переходе к регистрации
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (AuthUser(LoginText.Text, PassText.Password))
            {
                if (user.IsFreeze == true)
                {
                    MessageBox.Show("Ваш аккаунт заблокирован.", "Ошибка");
                    return;
                }

                MainWindow main = new MainWindow(user);
                main.Show();
                this.Close(); // Закрываем окно входа после успешного входа
            }
        }

        public bool AuthUser(string login, string password)
        {
            var UsInDB = Core.Context.user_.FirstOrDefault(u => u.Login == login);
            if (UsInDB != null && UsInDB.Password == password)
            {
                user = UsInDB;
                return true;
            }
            MessageBox.Show("Неверный логин или пароль!");
            return false;
        }
    }
}
