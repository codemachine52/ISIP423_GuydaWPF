using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WpfApp1.Pages;
namespace WpfApp1.Pages
{
    public partial class Page1 : Page
    {
        private List<Film> allMovies = Core.Context.Film.ToList();
        Client User;

        public Page1()
        {
            InitializeComponent();
            listBox.ItemsSource = allMovies;
        }

        public Page1(Client us) : this()
        {
            User = us;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (User == null)
            {
                NavigationService.Navigate(new Page3());
            }
            else
            {
                NavigationService.Navigate(new Page4(User));
            }
        }
    }
}