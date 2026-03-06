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
    /// Логика взаимодействия для Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        public Client us { get; set; }
        public Page4(Client user)
        {
            InitializeComponent();
            us = user;
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            if (us != null)
            {
                FIOuser.Text += " " + us.Fio;
                AGEuser.Text += " " + us.Age;
                EMAILuser.Text += " " + us.Email;
                NUMBERuser.Text += " " + us.PhoneNum;
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1());
        }
    }
}
