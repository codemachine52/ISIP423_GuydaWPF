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
    /// Логика взаимодействия для ModelEngine.xaml
    /// </summary>
    public partial class ModelEngine : Page
    {
        public ModelEngine()
        {
            InitializeComponent();

            var cra = NavigationData.CurrentData as Car;
            cra.Model = new Option
            {
                name = "Default",
                Price = 0
            };
            cra.Engine = new Option
            {
                name = "Default",
                Price = 0
            };
            NavigationData.CurrentData = cra;

            RecountTotal();


            List<Option> engines = new List<Option>()
            {
                new Option
                {
                    Price = 200000,
                    name = "S55 - самый надежный"
                },
                new Option
                {
                    name = "Атмосферный 2.0",
                    Price = 93000
                },
                new Option
                {
                    name = "6.3 V12 - идеально для повседневной езды",
                    Price = 500000
                }
            };
            List<Option> models = new List<Option>()
            {
                new Option
                {
                    Price = 2000000,
                    name = "BMW X5 E53 2007"
                },
                new Option
                {
                    Price = 4589000,
                    name = "Audi RS Q7 2016"
                },
                new Option
                {
                    Price = 6700000,
                    name = "Mercedes Benz W213 AMG рестайлинг"
                    
                }
            };

            ModelComboBox.ItemsSource = models;
            ModelComboBox.DisplayMemberPath = "name";
            ModelComboBox.SelectedIndex = 0;

            EngineComboBox.ItemsSource = engines;
            EngineComboBox.DisplayMemberPath = "name";
            EngineComboBox.SelectedIndex = 0;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var car = NavigationData.CurrentData as Car;
            car.Model = (Option)ModelComboBox.SelectedItem;
            car.Engine = (Option)EngineComboBox.SelectedItem;
            //car.TotalPrice = ((Option)ModelComboBox.SelectedItem).Price + ((Option)EngineComboBox.SelectedItem).Price;

            NavigationData.CurrentData = car;

            NavigationService.Navigate(new ColorOptions());
        }

        private void ModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModelCost.Text = "Стоимость: " + ((Option)ModelComboBox.SelectedItem).Price;
            if (EngineComboBox.SelectedIndex != -1) TotalCost.Text = "Общая Стоимость: " + (((Option)ModelComboBox.SelectedItem).Price + ((Option)EngineComboBox.SelectedItem).Price);
        }

        private void EngineComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineCost.Text = "Стоимость: " + ((Option)EngineComboBox.SelectedItem).Price;
            TotalCost.Text = "Общая Стоимость: " + (((Option)ModelComboBox.SelectedItem).Price + ((Option)EngineComboBox.SelectedItem).Price);
        }

        public void RecountTotal()
        {
            var cra = NavigationData.CurrentData as Car;
            cra.TotalPrice = 0;
            cra.TotalPrice += cra.Model.Price;
            cra.TotalPrice += cra.Engine.Price;
            cra.TotalPrice += cra.Color.Price;
            foreach (var i in cra.More)
            {
                cra.TotalPrice += i.Price;
            }
            NavigationData.CurrentData = cra;
        }
    }
}
