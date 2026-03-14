using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для Page6.xaml
    /// </summary>
    public partial class Page6 : Page
    {
        public class SeatInfo
        {
            public Place Place { get; set; }
            public bool IsBusy { get; set; }
        }

        public class RowInfo
        {
            public int LineNumber { get; set; }
            public List<SeatInfo> Seats { get; set; }
        }


        public Session session { get; set; }

        public HashSet<int> BusyPlaceIds { get; set; } // HashSet для быстрой проверки
        public Client user { get; set; }

        public Page6(Client us)
        {
            InitializeComponent();
        }

        public Page6(int sion, Client cl) : base()
        {
            InitializeComponent();
            session = Core.Context.Session.Where(s => s.ID == sion).FirstOrDefault();
            user = cl;
            LoadSeats();
        }

        private void LoadSeats()
        {
            var allSts = Core.Context.Place.Where(p => p.IDhall == session.IDhall).OrderBy(p => p.Line).ThenBy(s => s.Seat).ToList();

            var busyIds = Core.Context.BusyPlace
         .Where(bp => bp.IDSession == session.ID && bp.IsBusy == true)
         .Select(bp => bp.IDPlace)
         .ToHashSet();

            // Превращаем места в SeatInfo
            var seatInfos = allSts.Select(p => new SeatInfo
            {
                Place = p,
                IsBusy = busyIds.Contains(p.ID)
            }).ToList();

            var rows = seatInfos.GroupBy(si => si.Place.Line).Select(g => new RowInfo
                                                                {
                                                                    LineNumber = g.Key,
                                                                    Seats = g.ToList()
                                                                }).ToList();

            DataContext = new { Rows = rows };
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1(user));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var seatInfo = btn?.DataContext as SeatInfo;
            if (seatInfo == null) return;

            if (seatInfo.IsBusy) // на всякий случай, хотя кнопка уже disabled
            {
                MessageBox.Show("Место занято!");
                return;
            }

            NavigationService.Navigate(new Page8(user, seatInfo.Place, session));
        }
    }
}