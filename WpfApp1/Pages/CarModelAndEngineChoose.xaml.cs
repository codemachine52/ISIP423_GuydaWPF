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

            

            EngineChoose.SelectedIndex = 0;
            EngineChoose.ItemsSource = engines;
            EngineChoose.DisplayMemberPath = "HP";
        }

        private void CarChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EngineChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(EngineChoose.SelectedIndex == 0)
                MessageBox.Show($"Вы выбрали двигатель {engines[0].HP} лошадинных сил, объемом {engines[0].displacement}!");
            if (EngineChoose.SelectedIndex == 1)
                MessageBox.Show($"Вы выбрали двигатель {engines[1].HP} лошадинных сил, объемом {engines[1].displacement}!");
            if (EngineChoose.SelectedIndex == 2)
                MessageBox.Show($"Вы выбрали двигатель {engines[2].HP} лошадинных сил, объемом {engines[2].displacement}!");
        }
    }
}
