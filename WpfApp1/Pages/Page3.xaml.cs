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
        public bool EmPass { get; set; }
        public bool passwordPass {  get; set; }
        public bool FioPass { get; set; }
        public bool PhonePass { get; set; }
        public bool AgePass { get; set; }
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
            bool isAllFilled = !string.IsNullOrWhiteSpace(LoginEnter.Text) &&
                               !string.IsNullOrWhiteSpace(PasswordEnter.Password) &&
                               !string.IsNullOrWhiteSpace(FIOEnter.Text) &&
                               user.Age > 0 &&
                               !string.IsNullOrWhiteSpace(PhoneEnter.Text);

            Registr.IsEnabled = isAllFilled;
        }

        private void Page3_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Registr_Click(object sender, RoutedEventArgs e)
        {
            string email = LoginEnter.Text;
            string password = PasswordEnter.Password;
            string fio = FIOEnter.Text;
            string phone = PhoneEnter.Text;
            if (!int.TryParse(AgeEnter.Text, out int age))
            {
                MessageBox.Show("Возраст должен быть числом!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (RegistrationUser(email, password, fio, age, phone))
            {
                NavigationService.Navigate(new Page1(user));
            }
            else
            {
                MessageBox.Show("Ошибка! Регистрация не удалась, есть ошибки в заполнении полей.", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool RegistrationUser(string email, string pass, string FIO, int age, string phoneNum)
        {
            var usIsInBD = Core.Context.Client.Where(u => u.Email == email).FirstOrDefault();

            if (usIsInBD == null)
            {
                if (CheckFields(email, pass, FIO, age, phoneNum))
                {
                    Core.Context.Client.Add(user);
                    Core.Context.SaveChanges();
                    MessageBox.Show("Пользователь успешно зарегистрирован!", "Успешная регистрация");
                    return true;
                }
                else return false;
            }
            else
            {
                MessageBox.Show("Пользователь уже зарегистрирован!", "Повторная регистрация", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                return false;
            }
        }

        private bool CheckFields(string email, string pass, string FIO, int age, string phoneNum)
        {
            while (true)
            {
                // Проверка email
                if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                {
                    MessageBox.Show("Почта должна быть указана и содержать '@'!", "Некорректный ввод",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                // Проверка пароля
                if (string.IsNullOrEmpty(pass) || pass.Length <= 5)
                {
                    MessageBox.Show("Пароль должен содержать больше 5 символов и не быть пустым!", "Некорректный ввод",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                // Проверка ФИО
                if (string.IsNullOrEmpty(FIO))
                {
                    MessageBox.Show("Поле ФИО должно быть заполнено!", "Некорректный ввод",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                // Проверка возраста
                if (age <= 0)
                {
                    MessageBox.Show("Возраст должен быть положительным числом!", "Некорректный ввод",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                // Проверка телефона
                if (string.IsNullOrEmpty(phoneNum))
                {
                    MessageBox.Show("Номер телефона не может быть пустым!", "Некорректный ввод",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                // Нормализация номера (если есть +, то должно быть 12 символов, иначе 11)
                string cleanPhone = phoneNum.Trim();
                bool isPhoneValid = (cleanPhone.StartsWith("+") && cleanPhone.Length == 12) ||
                                    (!cleanPhone.StartsWith("+") && cleanPhone.Length == 11);

                if (!isPhoneValid)
                {
                    MessageBox.Show("Номер телефона должен содержать 11 цифр или 12 с '+' в начале!", "Некорректный ввод",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                // Все проверки пройдены – сохраняем данные в объект пользователя
                user.Email = email;
                user.Password = pass;
                user.Fio = FIO;
                user.Age = age;
                user.PhoneNum = phoneNum; // Можно сохранять как есть, либо нормализовать

                return true;
            }
        }

        private void LoginEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(LoginEnter.Text))
            {
                if (LoginEnter.Text.Contains("@"))
                {
                    user.Email = LoginEnter.Text;
                    EmPass = true;
                }
                else
                {
                    MessageBox.Show("Проверьте правильность введенной почты.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                    EmPass = false;
                }
            }
            else MessageBox.Show("Ошибка! Почта должна быть указана и содержать '@'!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
            CheckAllFieldsFilled();
            EmPass = false;
            
        }

        private void PasswordEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            string pasEnt = PasswordEnter.Password;
            if (!string.IsNullOrEmpty(pasEnt))
            {
                if (pasEnt.Length > 5)
                {
                    user.Password = pasEnt;
                    passwordPass = true;
                }
                else
                {
                    MessageBox.Show("Пароль должен содержать больше 5 символов!");
                    CheckAllFieldsFilled();
                    passwordPass = false;
                }
            }
            else MessageBox.Show("Ошибка! пароль не может быть пустым", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            CheckAllFieldsFilled();
            passwordPass = false;
            
        }

        private void FIOEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FIOEnter.Text))
            {
                user.Fio = FIOEnter.Text;
                FioPass = true;
            }
            else MessageBox.Show("Ошибка! Поле ФИО должно быть заполнено!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            CheckAllFieldsFilled();
            FioPass = false;
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
                        AgePass = true;
                    }
                    else MessageBox.Show("Ошибка! Возраст не может быть отрицательным!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
                    AgePass = false;
                }
                else MessageBox.Show("Ошибка! Возраст - это число! Введите в поле числовое значение.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                AgePass = false;
            }
            else MessageBox.Show("Ошибка! Поле не может быть пустым!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            CheckAllFieldsFilled();
            AgePass = false;
        }

        private void PhoneEnter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PhoneEnter.Text))
            {
                if (PhoneEnter.Text.Contains("+"))
                {
                    if (PhoneEnter.Text.Length == 12)
                    {
                        user.PhoneNum = PhoneEnter.Text;
                        PhonePass = true;
                    }
                    else
                    {
                        MessageBox.Show("Проверьте правильность введенного номера.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                        PhonePass = false;
                    }
                }
                else if (PhoneEnter.Text.Length == 11)
                {
                    user.PhoneNum = PhoneEnter.Text;
                    PhonePass = true;
                }

                else
                {
                    MessageBox.Show("Проверьте правильность введенного номера телефона.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
                    PhonePass = false;
                }
            }
            else MessageBox.Show("Ошибка! Номер телефона не может быть пустым!", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Error);
            CheckAllFieldsFilled();
            PhonePass = false;
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
