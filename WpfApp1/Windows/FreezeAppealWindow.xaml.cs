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
        public FreezeAppealWindow(user_ user, string reason)
        {
            InitializeComponent();
            _user = user;
            TBlockReason.Text = reason;
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBoxAppeal.Text))
            {
                MessageBox.Show("Введите текст обращения");
                return;
            }

            // Создаем запись в таблице апелляций
            Core.Context.requestUnFreeze.Add(new requestUnFreeze
            {
                userID = _user.ID,
                requestText = TBoxAppeal.Text,
             });

            Core.Context.SaveChanges();
            MessageBox.Show("Апелляция отправлена. Ожидайте решения модератора.");
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
