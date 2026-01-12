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
using WpfApp1.Model;
using System.Security.Cryptography.X509Certificates;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для OptionsCar.xaml
    /// </summary>
    
    public partial class OptionsCar : Page
    {
        List<Options> options = new List<Options>()
    {
        new Options
        {
            name = "Кондиционер",
            price = 35000
        },
        new Options
        {
            name = "Климат-контроль",
            price = 70000
        },
        new Options
        {
            name = "Гидравлика",
            price = 150000
        },
        new Options
        {
            name = "Тонировка",
            price = 26500
        }
    };

        decimal price1;
        Car machine;
        public OptionsCar(Car car)
        {
            InitializeComponent();
            price1 = car.price;
            machine = car;
            Calculate();
        }
        private void Condey_Checked(object sender, RoutedEventArgs e)
        {
            price1 += options[0].price;
            Calculate();
        }

        private void Climat_Checked(object sender, RoutedEventArgs e)
        {
            price1 += options[1].price;
            Calculate();
        }

        private void Gidrl_Checked(object sender, RoutedEventArgs e)
        {
            price1 += options[2].price;
            Calculate();
        }

        private void Toner_Checked(object sender, RoutedEventArgs e)
        {
            price1 += options[3].price;
            Calculate();
        }
        public void Calculate()
        {
            CarWithOptionPrice.Text = $"Стоимость машины с доп. опциями: {price1.ToString()}";
        }
    }
}
