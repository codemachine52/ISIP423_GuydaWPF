using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<Film> _allMovies;              // все фильмы из БД
        private ObservableCollection<Film> _displayedMovies; // для отображения в ListBox
        Client User;

        public Page1()
        {
            InitializeComponent();
            _allMovies = Core.Context.Film.ToList();
            _displayedMovies = new ObservableCollection<Film>(_allMovies);
            listBox.ItemsSource = _displayedMovies;
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
            if(User != null)
            {
                NavigationService.Navigate(new Page4(User));
            }
        }

        private void ButInfo_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Film thisFilm = btn.DataContext as Film;
            NavigationService.Navigate(new Page5(thisFilm));
        }
        private void FindingFilm()
        {
            string searchText = FindFilm.Text.Trim().ToLower();
            _displayedMovies.Clear();
            if (string.IsNullOrEmpty(searchText))
            {
                // Если поиск пустой – показываем все фильмы
                foreach (var film in _allMovies)
                    _displayedMovies.Add(film);
                return;
            }

            var foundFilms = _allMovies.Where(item =>
                item.Name.Trim().ToLower().Contains(searchText)).ToList();

            // Если фильм найден, добавляем его в ListBox
            foreach (var film in foundFilms)
                _displayedMovies.Add(film);
        }

        private void FindFilm_TextChanged(object sender, TextChangedEventArgs e)
        {
            FindingFilm();
        }
    }
}