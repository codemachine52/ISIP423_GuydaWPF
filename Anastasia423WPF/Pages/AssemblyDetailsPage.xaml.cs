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
    /// Логика взаимодействия для AssemblyDetailsPage.xaml
    /// </summary>
    public partial class AssemblyDetailsPage : Page
    {
        private assembly_ _currentAssembly;

        public AssemblyDetailsPage(assembly_ selectedAssembly)
        {
            InitializeComponent();
            _currentAssembly = selectedAssembly;

            // Заполняем заголовки
            TxtAssemblyName.Text = _currentAssembly.name;
            TxtAuthor.Text = $"Автор: {_currentAssembly.author}";

            LoadParts();
        }

        private void LoadParts()
        {
            // Ищем все записи в связующей таблице для нашей сборки
            // и вытаскиваем сами детали (basepart)
            var parts = Core.Context.partassembly_
                .Where(pa => pa.assemblyid == _currentAssembly.id)
                .Select(pa => pa.basepart_)
                .ToList();

            PartsListView.ItemsSource = parts;

            // Считаем общую сумму
            decimal totalPrice = parts.Sum(p => p.price);
            TxtTotalPrice.Text = $"{totalPrice:N0} руб.";
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаемся назад
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
