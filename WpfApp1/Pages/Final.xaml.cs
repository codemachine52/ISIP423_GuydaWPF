using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
using System.Xml.Linq;

namespace Pr12.Pages
{
    /// <summary>
    /// Логика взаимодействия для Final.xaml
    /// </summary>
    public partial class Final : Page
    {
        public Final()
        {
            InitializeComponent();
            var _data = NavigationData.CurrentData as Car;
            TotalPrice.Text = Convert.ToString(_data.TotalPrice);
            ModelText.Text += _data.Model.name + " " + _data.Model.Price + " Руб";
            EngineText.Text += _data.Engine.name + " " + _data.Engine.Price + " Руб";
            ColorText.Text += _data.Color.name + " " + _data.Color.Price + " Руб";
            foreach (var i in _data.More)
            {
                MoreText.Text += "\n" + "- " + i.name + " " + i.Price + " Руб";
            }
            NameText.Text += _data.Name;
            PhoneText.Text += _data.Phone;
            EmailText.Text += _data.Email;
            PerText.Text += _data.Percent + "%";
            MonText.Text += _data.Months;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string writeText = "";
            var _data = NavigationData.CurrentData as Car;
            writeText = "Всего: " + Convert.ToString(_data.TotalPrice) + " Руб\n";
            writeText += _data.Model.name + " " + _data.Model.Price + " Руб\n";
            writeText += _data.Engine.name + " " + _data.Engine.Price + " Руб\n";
            writeText += _data.Color.name + " " + _data.Color.Price + " Руб\n";

            foreach (var i in _data.More)
            {
                writeText += "- " + i.name + " " + i.Price + " Руб\n";
            }

            writeText += _data.Name + "\n";
            writeText += _data.Email + "\n";
            writeText += _data.Phone + "\n";
            File.WriteAllText("save.txt", writeText);

            Application.Current.Shutdown();
        }
    }
}
