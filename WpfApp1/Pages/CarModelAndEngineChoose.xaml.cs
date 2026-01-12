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
    public partial class CarModelAndEngineChoose : Page
    {
        public CarModelAndEngineChoose()
        {
            InitializeComponent();
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

            CarChoose.ItemsSource = cars;
            CarChoose.SelectedIndex = 0;
            CarChoose.DisplayMemberPath = "Name";


        }

        private void CarChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
