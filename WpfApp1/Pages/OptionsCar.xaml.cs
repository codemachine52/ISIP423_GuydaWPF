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
        },
    };
        List<Options> colors = new List<Options>()
        {
            new Options
        {
            name = "Синий",
            price = 7200
        },
        new Options
        {
            name = "Желтый",
            price = 4500
        },
        new Options
        {
            name = "Фиолетовый",
            price = 10000
        },
        new Options
        {
            name = "Пурпурный",
            price = 11000
        },
        new Options
        {
            name = "Огненный",
            price = 9900
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
            Colors.ItemsSource = colors;
            Colors.DisplayMemberPath = "name";
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

        private void Colors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Calculate();
        }
    }
}
