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
    /// Логика взаимодействия для Choices.xaml
    /// </summary>
    public partial class Choices : Page
    {
        public Choices()
        {
            InitializeComponent();
            RecountTotal();
            var _data = NavigationData.CurrentData as Car;
            TotalPrice.Text = Convert.ToString(_data.TotalPrice);
            ModelText.Text += _data.Model.name + " " + _data.Model.Price + " Руб";
            EngineText.Text += _data.Engine.name + " " + _data.Engine.Price + " Руб";
            ColorText.Text += _data.Color.name + " " + _data.Color.Price + " Руб";
            foreach (var i in _data.More)
            {
                MoreText.Text += "\n" + "- " + i.name + " " + i.Price + " Руб";
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Credit());
        }
    }
}
