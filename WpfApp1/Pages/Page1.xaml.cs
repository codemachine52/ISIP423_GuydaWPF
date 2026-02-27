using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
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
            RatingConvert();
        }
        public int ratconv;
        private void RatingConvert()
        {
            foreach (Film film in allMovies)
            {
                if(film.Rating >= 7)
                {
                    ratconv = 1;
                }
                if (film.Rating < 7 && film.Rating > 5) ratconv = 2;
                if (film.Rating < 5 && film.Rating > 3) ratconv = 3;
                else ratconv = 4;
            }
        }
    }
}