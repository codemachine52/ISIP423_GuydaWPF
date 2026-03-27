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

namespace Anastasia423WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для SavedBuildsPage.xaml
    /// </summary>
    public partial class SavedBuildsPage : Page
    {
        public SavedBuildsPage()
        {
            InitializeComponent();
            LoadAssemblies();
        }

        private void LoadAssemblies()
        {
            LwAssemblies.ItemsSource = Core.Context.assembly_.ToList();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void LwAssemblies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Получаем сборку, по которой кликнули
            if (LwAssemblies.SelectedItem is assembly_ selectedAssembly)
            {
                // Переходим на страницу деталей, передавая эту сборку
                NavigationService.Navigate(new AssemblyDetailsPage(selectedAssembly));
            }
        }
    }
}
