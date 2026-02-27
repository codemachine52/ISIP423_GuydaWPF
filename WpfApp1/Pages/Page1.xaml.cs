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

        public Page1()
        {
            InitializeComponent();
            listBox.ItemsSource = allMovies;
        }
        
    }
}