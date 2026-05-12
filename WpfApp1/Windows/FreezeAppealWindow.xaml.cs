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

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для FreezeAppealWindow.xaml
    /// </summary>
    public partial class FreezeAppealWindow : Window
    {
        private user_ _user;
        private int? _bookId; // Храним ID книги, если апелляция по ней

        public Visibility BookTitleVisibility => _bookId != null ? Visibility.Visible : Visibility.Collapsed;
        public Visibility AccountTitleVisibility => _bookId == null ? Visibility.Visible : Visibility.Collapsed;

        // Универсальный конструктор
        public FreezeAppealWindow(user_ user, string reason, int? bookId = null)
        {
            InitializeComponent();
            _user = user;
            _bookId = bookId;
            TBlockReason.Text = reason;

            // Меняем заголовок в зависимости от типа
            if (_bookId != null)
                this.Title = "Оспорить заморозку книги";
            else
                this.Title = "Оспорить заморозку аккаунта";
            this.DataContext = this;
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBoxAppeal.Text))
            {
                MessageBox.Show("Напишите текст апелляции");
                return;
            }

            // Сохраняем в БД (таблица requestUnFreeze)
            var request = new requestUnFreeze
            {
                userID = _user.ID,
                bookID = _bookId, // Если null - значит аккаунт, если число - значит книга
                requestText = TBoxAppeal.Text,
                reportDate = DateTime.Now
            };

            Core.Context.requestUnFreeze.Add(request);
            Core.Context.SaveChanges();

            MessageBox.Show("Ваше обращение отправлено модераторам.");
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
