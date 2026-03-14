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
    /// Логика взаимодействия для Page5.xaml
    /// </summary>
    public partial class Page5 : Page
    {
        public Film kino {  get; set; }
        public Client user { get; set; }
        public Page5(Film thisFilm, Client us)
        {
            InitializeComponent();
            kino = thisFilm;
            this.DataContext = thisFilm;
            Ganre Genre = Core.Context.Ganre.Where(g => thisFilm.GanreID == g.ID).FirstOrDefault();
            fGenre.Text += Genre.GanreName;
            var age = Core.Context.Age.Where(a => a.ID == kino.AgeID).FirstOrDefault().Number;
            AgeText.Text += age + "+";


            user = us;
        }
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1(user));
        }

        private void TicketBuy_Click(object sender, RoutedEventArgs e)
        {
            if (user != null)
            {
                var over18 = Core.Context.Age.Where(a => a.ID == kino.AgeID).FirstOrDefault().IsOver18;
                if(over18 && user.Age > 18)
                NavigationService.Navigate(new Page7(kino, user));
                if(over18 && user.Age < 18)
                {
                    MessageBox.Show("Ошибка! Вам еще нельзя смотреть такие фильмы! Выберите другой из нашего каталога.", "Несоответствие возраста", MessageBoxButton.OK, MessageBoxImage.Error);
                    NavigationService.Navigate(new Page1(user));
                }
                if (!over18)
                {
                    NavigationService.Navigate(new Page7(kino, user));
                }
            }
            else
            {
                MessageBox.Show("Необходимо войти в аккаунт перед покупкой билета!", "Неавторизованный пользователь", MessageBoxButton.OK, MessageBoxImage.Warning);
                NavigationService.Navigate(new Page2());
            }
        }
    }
}
