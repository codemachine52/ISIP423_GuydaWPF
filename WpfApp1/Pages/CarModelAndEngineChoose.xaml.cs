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
    /// Логика взаимодействия для CarModelAndEngineChoose.xaml
    /// </summary>
    /// 
    public partial class CarModelAndEngineChoose : Page
    {
        decimal PriceFinal;
        List<Engine> engines = new List<Engine>()
            {
                new Engine
                {
                    HP = 159,
                    displacement = 2.0,
                    cost = 100000
                },
                new Engine
                {
                    HP = 230,
                    displacement = 2.4,
                    cost = 350000
                },
                new Engine
                {
                    HP = 600,
                    displacement = 5.0,
                    cost = 678999
                }
            };
        List<Car> cars = new List<Car>()
            {
                new Car{
                    Name = "Мерс Е200 2003",
                    price = 1950000,
                    color = "Черный"
                },
                new Car{
                    Name = "БМВ Е60 2007",
                    price = 6000000,
                    color = "Серый хром"
                },
               new Car{
                    Name = "Ауди РС6 2017",
                    price = 4500000,
                    color = "Красный"
                }
            };
        public CarModelAndEngineChoose()
        {
            InitializeComponent();
            CarChoose.ItemsSource = cars;
            CarChoose.SelectedIndex = 0;
            CarChoose.DisplayMemberPath = "Name";


            EngineChoose.SelectedIndex = 0;
            EngineChoose.ItemsSource = engines;
            EngineChoose.DisplayMemberPath = "HP";
        }

        private void CarChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EngineChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EngineChoose.SelectedIndex != 0)
                MessageBox.Show($"Вы выбрали двигатель {engines[EngineChoose.SelectedIndex].HP} лошадинных сил, объемом {engines[EngineChoose.SelectedIndex].displacement} л.");
            
        }

        private void PriceCalculate_Click(object sender, RoutedEventArgs e)
        {
            PriceText.Text = "Итоговая цена: ";
            if(EngineChoose.SelectedIndex == 0)
            {
                PriceText.Text += cars[CarChoose.SelectedIndex].price.ToString();
            }
            else
            {
                PriceFinal = cars[CarChoose.SelectedIndex].price + engines[EngineChoose.SelectedIndex].cost;
                PriceText.Text += PriceFinal.ToString();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoForward == true)
            {
                //NavigationService.GoForward();
            }
            if (CarChoose.SelectedIndex != -1 && EngineChoose.SelectedIndex != -1)
            {
                var car = NavigationData.CurrentData as Car;
                car.Name = CarChoose.SelectedItem.ToString();
                car.engine = (Engine)EngineChoose.SelectedItem;
                car.price = PriceFinal;
                NavigationData.CurrentData = car;
                NavigationService.Navigate(new OptionsCar(car));
            }
        }
    }
}
