using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Pr12.Pages
{
    /// <summary>
    /// Логика взаимодействия для Results.xaml
    /// </summary>
    public partial class Results : Page
    {
        public Results()
        {
            InitializeComponent();
            Next.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var cra = NavigationData.CurrentData as Car;
            cra.Name = name.Text;
            cra.Phone = Phone.Text;
            cra.Email = Email.Text;
            NavigationData.CurrentData = cra;

            NavigationService.Navigate(new Final());
        }

        public void CheckTextBoxes()
        {
            int PhoneNum;
            bool PhoneGood = IsDigitsOnly(Phone.Text);
            bool NameGood = true;
            bool MailGood = true;

            if (PhoneGood) PhoneGood = (Phone.Text.Length > 8) && (Phone.Text.Length > 0);

            if (name.Text.Length > 5) NameGood = true;

            if (Email.Text.Length > 5 && Email.Text.Contains("@")) MailGood = true;

            if (PhoneGood && NameGood && MailGood) { Next.IsEnabled = true; }
            else { Next.IsEnabled = false; }
        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTextBoxes();
        }

        private void Phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTextBoxes();
        }

        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTextBoxes();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
