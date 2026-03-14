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
using System.Data.Entity;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page7.xaml
    /// </summary>
    /// 
    public partial class Page7 : Page
    {
        public Film flm {  get; set; }
        public Client cl { get; set; }
        //List<Session> sessions = Core.Context.Session.ToList();
        public Page7()
        {
            InitializeComponent();
        }

        public Page7(Film thisFilm, Client User) : this()
        {
            flm = thisFilm;
            cl = User;
            LoadData();
        }

        private void LoadData()
        {
            var SessionData = from session in Core.Context.Session
                              join hall in Core.Context.Hall on session.IDhall equals hall.ID
                              join classif in Core.Context.Classification on hall.ClassID equals classif.ID
                              where session.FilmID == flm.ID
                              select new SessionDisplay
                              {
                                  SessionID = session.ID,
                                  Time = session.TimeSession,
                                  Price = classif.Price,
                                  HallName = hall.HallName
                              };

            ViewModel viewModel = new ViewModel
            {
                Film = flm,
                sessions = SessionData.ToList()
            }; 

            this.DataContext= viewModel;
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1(cl));
        }

        private void ButSeans_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Tag is int SessionID)
            {
                NavigationService.Navigate(new Page6(SessionID, cl));
            }
        }
    }
}
