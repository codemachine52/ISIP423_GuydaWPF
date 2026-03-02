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
    /// Логика взаимодействия для Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
            Loaded += Page3_Loaded;
            Registr.IsEnabled = false;
        }
        Client user = new Client();
        private void CheckAllFieldsFilled()
        {
            // Проверяем что все поля user заполнены (не null и не пустые)
            bool isAllFilled = !string.IsNullOrWhiteSpace(user.Email) &&
                               !string.IsNullOrWhiteSpace(user.Password) &&
                               !string.IsNullOrWhiteSpace(user.Fio) &&
                               user.Age > 0 &&
                               !string.IsNullOrWhiteSpace(user.PhoneNum);

            Registr.IsEnabled = isAllFilled;
        }

        private void Page3_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Registr_Click(object sender, RoutedEventArgs e)
        {
            Core.Context.Client.Add(user);
            Core.Context.SaveChanges();
            if (Core.Context.Client.Contains(user))
            {
                MessageBox.Show("Пользователь успешно зарегистрирован!", "Успешная регистрация");
            }
        }

        private void LoginEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(LoginEnter.Text))
            {
                if (LoginEnter.Text.Contains("@"))
                {
                    user.Email = LoginEnter.Text;
                }
                else
                {
                    MessageBox.Show("Проверьте правильность введенной почты.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else MessageBox.Show("Ошибка! Почта должна быть указана и содержать '@'!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
            CheckAllFieldsFilled();
        }

        private void PasswordEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            string pasEnt = PasswordEnter.Password;
            if (!string.IsNullOrEmpty(pasEnt))
            {
                if (pasEnt.Length > 5)
                {
                    user.Password = pasEnt;
                }
            }
            else MessageBox.Show("Ошибка! пароль не может быть пустым и должен содержать больше 5 символов!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            CheckAllFieldsFilled();
        }

        private void FIOEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FIOEnter.Text))
            {
                user.Fio = FIOEnter.Text;
            }
            else MessageBox.Show("Ошибка! Поле ФИО должно быть заполнено!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            CheckAllFieldsFilled();
        }

        private void AgeEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AgeEnter.Text))
            {
                if (int.TryParse(AgeEnter.Text, out int usAge))
                {
                    if (usAge > 0)
                    {
                        user.Age = usAge;
                    }
                    else MessageBox.Show("Ошибка! Возраст не может быть отрицательным!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else MessageBox.Show("Ошибка! Возраст - это число! Введите в поле числовое значение.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else MessageBox.Show("Ошибка! Поле не может быть пустым!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            CheckAllFieldsFilled();
        }

        private void PhoneEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PhoneEnter.Text))
            {
                if (PhoneEnter.Text.Contains("+"))
                {
                    if (PhoneEnter.Text.Length == 12) user.PhoneNum = PhoneEnter.Text;
                    else MessageBox.Show("Проверьте правильность введенного номера.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (PhoneEnter.Text.Length == 11) user.PhoneNum = PhoneEnter.Text;

                else
                {
                    MessageBox.Show("Проверьте правильность введенного номера телефона.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else MessageBox.Show("Ошибка! Номер телефона не может быть пустым!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
            CheckAllFieldsFilled();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2(user));
        }
    }

    //button btn = sender as button;
    // Film thisFilm = btn.datacontext as film;

    // на след странице в конструктор добавляем фильм
    //глобальная переменная с get set
    // film = f в конструкторе
    // this.DataContext = this;
    // потом биндинг используем в xaml
}
