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
            if (Authenticate(LoginText.Text, PassText.Password))
            {
                MainWindow main = new MainWindow(user);
                main.Show();
                this.Close(); // Закрываем окно входа после успешного входа
            }
        }

        /// <summary>
        /// Авторизация пользователя в системе
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns>Возвращает true, если данные верны и аккаунт не заморожен</returns>
        public bool Authenticate(string login, string password)
        {
            try
            {
                var currentUser = Core.Context.user_
                    .FirstOrDefault(u => u.Login == login && u.Password == password);

                if (currentUser == null)
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                // Проверка статуса блокировки (перенесено из UI-логики в метод данных)
                if (currentUser.IsFreeze == true)
                {
                    // Ищем последнюю жалобу на этого пользователя, чтобы узнать причину
                    var lastReport = Core.Context.report
        .Where(r => r.userWasReportedID == currentUser.ID || (r.review != null && r.AuthorID == currentUser.ID))
        .OrderByDescending(r => r.ID)
        .FirstOrDefault();

                    string reason = "Нарушение правил платформы"; // Значение по умолчанию

                    if (lastReport != null)
                    {
                        // Определяем причину на основе заполненных полей в жалобе
                        if (lastReport.reviewID != null)
                        {
                            reason = $"Ваш отзыв к книге '{lastReport.review.book.Name}' был признан недопустимым.";
                        }
                        else if (lastReport.BookID != null)
                        {
                            reason = $"Ваше произведение '{lastReport.book.Name}' нарушает правила публикации.";
                        }
                        else if (lastReport.userWasReportedID != null)
                        {
                            reason = "Ваш профиль был заблокирован за нарушение правил сообщества.";
                        }
                    }

                    // Открываем окно апелляции
                    FreezeAppealWindow appealWin = new FreezeAppealWindow(currentUser, reason);
                    appealWin.ShowDialog();
                    return false; // Не пускаем в главное меню
                }
                Core.CurrentUser = currentUser;
                this.user = currentUser;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка БД: {ex.Message}");
                return false;
            }
        }
    }
}
