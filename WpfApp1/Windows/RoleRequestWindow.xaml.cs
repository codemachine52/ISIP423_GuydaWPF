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
    /// Логика взаимодействия для RoleRequestWindow.xaml
    /// </summary>
    public partial class RoleRequestWindow : Window
    {
        private int _userId;

        public RoleRequestWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBoxReviewText.Text))
            {
                MessageBox.Show("Напишите текст запроса!");
                return;
            }

            try
            {
                var newRequest = new requestRole
                {
                    userID = _userId,
                    requestText = TBoxReviewText.Text,
                    reportDate = DateTime.Now
                };

                Core.Context.requestRole.Add(newRequest);
                Core.Context.SaveChanges();

                MessageBox.Show("Запрос успешно отправлен на модерацию!");
                this.DialogResult = true; // Закрывает окно и сообщает об успехе
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
