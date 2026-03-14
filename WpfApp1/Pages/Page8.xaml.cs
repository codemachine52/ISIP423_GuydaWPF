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
    /// Логика взаимодействия для Page8.xaml
    /// </summary>
    public partial class Page8 : Page
    {
        public Client user {  get; set; }
        public Place place { get; set; }
        public Session session { get; set; }
        public Page8(Client us, Place pl, Session ses)
        {
            InitializeComponent();
            user = us;
            place = pl;
            session = ses;

            GetData();
        }

        private void GetData()
        {
            var hall = session.Hall;
            var placeName = place.Seat;
            var lineName = place.Line;
            var classifPrice = Core.Context.Classification.Where(c => hall.ClassID == c.ID).FirstOrDefault().Price;

            HallText.Text += hall.HallName;
            SeatText.Text += placeName.ToString();
            LineText.Text += lineName.ToString();
            PriceText.Text += classifPrice.ToString() + " рублей";
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page6(session.ID, user));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены в покупке билета?", "Покупка билета", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Поздравляем с покупкой билета! До встречи в кинотеатре! Ждем вас снова.");
                Ticket tkt = new Ticket()
                {
                    ClientID = user.ID,
                    SessionID = session.ID,
                    PlaceID = place.ID,
                };
                Core.Context.Ticket.Add(tkt);
                Core.Context.SaveChanges();
                BusyPlace busyPlace = new BusyPlace
                {
                    IDPlace = place.ID,
                    IDSession = session.ID,
                    IsBusy = true
                };
                Core.Context.BusyPlace.Add(busyPlace);
                Core.Context.SaveChanges();
                NavigationService.Navigate(new Page1(user));
            }
            else
            {
                MessageBox.Show("Жаль, что вы передумали. Возможно, вам понравится другой фильм в нашем кинотеатре!");
            }
        }
    }
}
