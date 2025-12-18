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
    /// Логика взаимодействия для ColorOptions.xaml
    /// </summary>
    public partial class ColorOptions : Page
    {
        public ColorOptions()
        {
            InitializeComponent();

            var cra = NavigationData.CurrentData as Car;
            cra.Color = new Option
            {
                name = "Default",
                Price = 0
            };
            cra.More = new List<Option>();
            NavigationData.CurrentData = cra;

            RecountTotal();

            List<Option> colors = new List<Option>()
            {
                new Option
                {
                    name = "Красный",
                    Price = 15000
                },
                new Option
                {
                    name = "Черный",
                    Price = 25000
                },
                new Option
                {
                    name = "Белый",
                    Price = 15000
                },
                new Option
                {
                    name = "Синий",
                    Price = 20000
                },
                new Option
                {
                    name = "Желтый",
                    Price = 10000
                },
                new Option
                {
                    name = "Серый",
                    Price = 15000
                },
                new Option
                {
                    name = "Голубой",
                    Price = 15000
                },
                new Option
                {
                    name = "Сиреневый",
                    Price = 13000
                },
                new Option
                {
                    name = "Мокрый асфальт",
                    Price = 25000
                }
            };

            ColorComboBox.ItemsSource = colors;
            ColorComboBox.DisplayMemberPath = "name";
            ColorComboBox.SelectedIndex = 0;
        }

        int moreSum = 0;
        List<Option> moreNames = new List<Option>();

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var car = NavigationData.CurrentData as Car;
            car.Color = (Option)ColorComboBox.SelectedItem;
            car.More = moreNames;
            //car.TotalPrice += ((Option)ColorComboBox.SelectedItem).Price + moreSum;


            NavigationData.CurrentData = car;
            NavigationService.Navigate(new Choices());
        }

        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ColorCost.Text = "Стоимость: " + ((Option)ColorComboBox.SelectedItem).Price;
            changeTotal();
        }

        private void more1_Checked(object sender, RoutedEventArgs e)
        {
            moreNames.Add(
                new Option
                {
                    name = "Доп.колесо",
                    Price = 5000
                }
                );
            changeTotal();
        }

        private void more2_Checked(object sender, RoutedEventArgs e)
        {
            moreNames.Add(
                new Option
                {
                    name = "Усилитель руля",
                    Price = 15000
                }
                );
            changeTotal();
        }

        private void more3_Checked(object sender, RoutedEventArgs e)
        {
            moreNames.Add(
                new Option
                {
                    name = "Круиз-контроль",
                    Price = 20000
                }
                );
            changeTotal();
        }

        private void more4_Checked(object sender, RoutedEventArgs e)
        {
            moreNames.Add(
                new Option
                {
                    name = "V12",
                    Price = 50000
                }
                );
            changeTotal();
        }

        void changeTotal()
        {
            var cra = NavigationData.CurrentData as Car;
            int Summ = 0;
            foreach (var opt in moreNames)
            {
                Summ += opt.Price;
            }
            TotalCost.Text = "Общая Стоимость: " + (Summ + ((Option)ColorComboBox.SelectedItem).Price);

            MoreCost.Text = "Стоимость: " + Summ;
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

        private void more1_Unchecked(object sender, RoutedEventArgs e)
        {
            moreNames.Remove(moreNames.First(x => x.name == "Доп.колесо"));
            changeTotal();
        }

        private void more2_Unchecked(object sender, RoutedEventArgs e)
        {
            moreNames.Remove(moreNames.First(x => x.name == "Усилитель руля"));
            changeTotal();
        }

        private void more3_Unchecked(object sender, RoutedEventArgs e)
        {
            moreNames.Remove(moreNames.First(x => x.name == "Круиз-контроль"));
            changeTotal();
        }

        private void more4_Unchecked(object sender, RoutedEventArgs e)
        {
            moreNames.Remove(moreNames.First(x => x.name == "V12"));
            changeTotal();
        }
    }
}
