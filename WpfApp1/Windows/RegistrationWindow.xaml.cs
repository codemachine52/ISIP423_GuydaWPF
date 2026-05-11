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
using WpfApp1.Pages;

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private user_ us = new user_();

        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWin = new AuthWindow();
            authWin.Show();
            this.Close();
        }

        //Обработчики потери фокуса (Inline-валидация)
        private void LoginText_LostFocus(object sender, RoutedEventArgs e) => ValidateLogin();
        private void PassText_LostFocus(object sender, RoutedEventArgs e) => ValidatePassword();
        private void PassVerificText_LostFocus(object sender, RoutedEventArgs e) => ValidatePasswordMatch();
        private void FIOText_LostFocus(object sender, RoutedEventArgs e) => ValidateFIO();
        private void PhoneText_LostFocus(object sender, RoutedEventArgs e) => ValidatePhone();


        private void ShowError(TextBlock errorBlock, TextBox input, string message)
        {
            errorBlock.Text = message;
            errorBlock.Visibility = Visibility.Visible;
            input.BorderBrush = Brushes.Red;
            input.BorderThickness = new Thickness(2);
        }

        private void ShowError(TextBlock errorBlock, PasswordBox input, string message)
        {
            errorBlock.Text = message;
            errorBlock.Visibility = Visibility.Visible;
            input.BorderBrush = Brushes.Red;
            input.BorderThickness = new Thickness(2);
        }

        private void HideError(TextBlock errorBlock, Control input)
        {
            errorBlock.Visibility = Visibility.Collapsed;
            input.ClearValue(Control.BorderBrushProperty);
            input.ClearValue(Control.BorderThicknessProperty);
        }

        private bool ValidateLogin()
        {
            if (string.IsNullOrWhiteSpace(LoginText.Text))
            {
                ShowError(LoginError, LoginText, "Логин не может быть пустым");
                return false;
            }

            // Проверка на занятость логина в БД прямо при вводе
            var existingUser = Core.Context.user_.FirstOrDefault(u => u.Login == LoginText.Text);
            if (existingUser != null)
            {
                ShowError(LoginError, LoginText, "Этот логин уже занят");
                return false;
            }

            HideError(LoginError, LoginText);
            return true;
        }

        private bool ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(PassText.Password))
            {
                ShowError(PassError, PassText, "Пароль не может быть пустым");
                return false;
            }
            if (PassText.Password.Length < 4)
            {
                ShowError(PassError, PassText, "Пароль слишком короткий (минимум 4 символа)");
                return false;
            }
            HideError(PassError, PassText);

            // Если подтверждение уже введено, проверяем совпадение заново
            if (!string.IsNullOrEmpty(PassVerificText.Password))
                ValidatePasswordMatch();

            return true;
        }

        private bool ValidatePasswordMatch()
        {
            if (PassText.Password != PassVerificText.Password)
            {
                ShowError(PassVerificError, PassVerificText, "Пароли не совпадают");
                return false;
            }
            HideError(PassVerificError, PassVerificText);
            return true;
        }

        private bool ValidateFIO()
        {
            if (string.IsNullOrWhiteSpace(FIOText.Text))
            {
                ShowError(FIOError, FIOText, "Укажите ваше ФИО");
                return false;
            }
            HideError(FIOError, FIOText);
            return true;
        }

        private bool ValidatePhone()
        {
            string cleanEmail = EmailText.Text.Trim();
            if (string.IsNullOrWhiteSpace(cleanEmail))
            {
                ShowError(EmailError, EmailText, "Укажите номер телефона");
                return false;
            }

            bool isEmailValid = cleanEmail.Contains("@");


            if (!isEmailValid)
            {
                ShowError(EmailError, EmailText, "Формат: user@mail.ru");
                return false;
            }

            HideError(EmailError, EmailText);
            return true;
        }

        private void RegistrUser_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = ValidateLogin() & ValidatePassword() & ValidatePasswordMatch() & ValidateFIO() & ValidatePhone();

            if (isValid)
            {
                user_ us = new user_
                {
                    Login = LoginText.Text,
                    Password = PassText.Password,
                    Name = FIOText.Text,
                    Email = EmailText.Text,
                    RoleID = 1,
                    IsFreeze = false
                };

                try
                {
                    Core.Context.user_.Add(us);
                    Core.Context.SaveChanges();
                    MessageBox.Show("Успех!");

                    MainWindow main = new MainWindow(us);
                    main.Show();
                    this.Close();
                }
                catch (System.Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
    }
}
