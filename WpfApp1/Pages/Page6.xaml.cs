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
    /// Логика взаимодействия для Page6.xaml
    /// </summary>
    public partial class Page6 : Page
    {
        public class RowInfo
        {
            public int LineNumber { get; set; }
            public List<Place> Seats { get; set; }
        }

        public Session session {  get; set; }
        public Client user { get; set; }
        public Page6(int sion, Client us)
        {
            InitializeComponent();
            session = Core.Context.Session.Where(s=> s.ID == sion).FirstOrDefault();
            user = us;
            LoadSeats();
        }

        private void LoadSeats()
        {
            var allSts = Core.Context.Place.Where(p => p.IDhall == session.IDhall).OrderBy(p => p.Line).ThenBy(s => s.Seat).ToList();
            var rows = allSts                     // список всех мест
    .GroupBy(p => p.Line)                 // группируем по номеру ряда (Line)
    .Select(g => new RowInfo               // для каждой группы создаём объект RowInfo
    {
        LineNumber = g.Key,                // Key — это номер ряда, по которому сгруппировали
        Seats = g.ToList()                  // g — это сама группа (все места этого ряда), превращаем её в список
    })
    .ToList();                              // превращаем результат в список
            DataContext = new { Rows = rows };
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1(user));
        }

    }
}
