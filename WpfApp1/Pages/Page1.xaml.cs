using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    public partial class Page1 : Page
    {
        private List<Film> allMovies = Core.Context.Film.ToList();

        public Page1()
        {
            InitializeComponent();
            listBox.ItemsSource = allMovies;

            FindFilm.TextChanged += (s, e) =>
            {
                SearchPlaceholder.Visibility =
                    string.IsNullOrWhiteSpace(FindFilm.Text) ?
                    Visibility.Visible : Visibility.Collapsed;
            };
        }
        private void FindFilm_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FindFilm.Text))
            {
                listBox.ItemsSource = allMovies;
            }
            else
            {
                var searchText = FindFilm.Text.ToLower();
                var filtered = allMovies.Where(m =>
                    m.FilmName.ToLower().Contains(searchText));
                listBox.ItemsSource = filtered;
            }
        }
            private void BuyTicket_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int filmId)
            {
                var film = allMovies.FirstOrDefault(f => f.ID == filmId);
                if (film != null)
                {
                    // Создаем объект Session для покупки
                    // Здесь можно открыть окно выбора сеанса
                    var result = MessageBox.Show(
                        $"Выбрать сеанс для фильма:\n\n" +
                        $"🎬 {film.FilmName}\n" +
                        $"⭐ Рейтинг: {film.Rating}/10\n" +
                        $"🎭 Жанр: {film.Ganre}\n" +
                        $"Возраст: {film.Age}\n" +
                        $"Перейти к выбору времени и места?",
                        "Покупка билета",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Здесь можно открыть страницу выбора сеанса
                        // Например: NavigationService.Navigate(new SessionPage(filmId));
                        MessageBox.Show($"Функционал выбора сеанса для фильма '{film.FilmName}' в разработке.",
                            "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        // Кнопка "Подробнее"
        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int filmId)
            {
                var film = allMovies.FirstOrDefault(f => f.ID == filmId);
                if (film != null)
                {
                    MessageBox.Show(
                        $"🎬 Название: {film.FilmName}\n" +
                        $"⭐ Рейтинг: {film.Rating}/10\n" +
                        $"🎭 Жанр: {film.Ganre}\n" +
                        $"🎫 Возрастной рейтинг: {film.Age}\n\n" +
                        $"📖 Описание:\n{film.Description}\n\n",
                        "Подробная информация о фильме",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        }

        // Кнопка профиля
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var user = Core.Context.Client.FirstOrDefault();
            // Проверяем авторизацию через Core
            if (user != null)
            {
                MessageBox.Show(
                    $"👤 Профиль пользователя:\n\n" +
                    $"ФИО: {user.Fio}\n" +
                    $"Email: {user.Email}\n" +
                    $"Телефон: {user.PhoneNum}\n" +
                    $"Возраст: {user.Age} лет",
                    "Мой профиль",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Для просмотра профиля необходимо авторизоваться!",
                    "Авторизация", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Сброс фильтров
        //private void ResetFilters_Click(object sender, RoutedEventArgs e)
        //{
        //    FindFilm.Text = "";
        //    GenreComboBox.SelectedIndex = 0;
        //    AgeComboBox.SelectedIndex = 0;
        //    listBox.ItemsSource = allFilms;
        //}

        // Добавление фильма в избранное
        private void AddToFavorites_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int filmId)
            {
                var film = allMovies.FirstOrDefault(f => f.ID == filmId);
                if (film != null)
                {
                    // Здесь можно добавить логику добавления в избранное
                    MessageBox.Show($"Фильм '{film.FilmName}' добавлен в избранное!",
                        "Избранное", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Меняем иконку кнопки
                    button.Content = "❤️ В избранном";
                    button.IsEnabled = false;
                }
            }
        }

        
    }
}