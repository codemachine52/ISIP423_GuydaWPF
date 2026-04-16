using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity; 

namespace Anastasia423WPF.Pages
{
    public partial class MasterCabinetPage : Page
    {
        private User _master;

        public MasterCabinetPage(User user)
        {
            InitializeComponent();
            _master = user;
            this.DataContext = _master;
            LoadSchedule();
        }

        private void LoadSchedule()
        {
            // Берем записи именно для этого мастера и подтягиваем названия услуг и имена клиентов
            var myAppointments = Core.Context.Apointment
                .Where(a => a.MasterID == _master.ID)
                .Include(a => a.Service)
                .Include(a => a.User) // В БД связь ClientID к таблице User
                .OrderBy(a => a.AppointmentDate)
                .ToList();

            ScheduleList.ItemsSource = myAppointments;
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }
    }
}