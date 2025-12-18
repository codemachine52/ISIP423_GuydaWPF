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

namespace Pr12.Pages
{
    /// <summary>
    /// Логика взаимодействия для Credit.xaml
    /// </summary>
    public partial class Credit : Page
    {
        public Credit()
        {
            InitializeComponent();
        }

        bool percNum = false;
        bool monNum = false;

        private void Percent_TextChanged(object sender, TextChangedEventArgs e)
        {
            float result;
            percNum = float.TryParse(Percent.Text, out result);
            if (percNum)
            {
                var cra = NavigationData.CurrentData as Car;
                cra.Percent = result;
                NavigationData.CurrentData = cra;
                Change_Values();
            }
        }

        private void months_TextChanged(object sender, TextChangedEventArgs e)
        {
            int result;
            monNum = int.TryParse(months.Text, out result);
            if (monNum)
            {
                var cra = NavigationData.CurrentData as Car;
                cra.Months = result;
                NavigationData.CurrentData = cra;
                Change_Values();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Results());
        }

        public void Change_Values()
        {
            if (Percent.Text.Length > 0 && months.Text.Length > 0 && monNum && percNum)
            {
                var cra = NavigationData.CurrentData as Car;
                vznos.Text = "Первоначальный взнос: " + ((cra.TotalPrice) * (cra.Percent / 100));

                Summa.Text = "Сумма в кредит: " + ((float)(cra.TotalPrice) - ((float)(cra.TotalPrice) * (cra.Percent / 100)));

                double S = ((double)(cra.TotalPrice) - ((double)(cra.TotalPrice) * (cra.Percent / 100)));

                double i = 0.15 / 20;

                double n = cra.Months;

                monthly.Text = "Ежемесячный платеж: " + (S * (i * Math.Pow(1 + i, n)) / Math.Pow(1 + i, n - 1));

            }
        }
    }
}
