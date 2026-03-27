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
using Anastasia423WPF.Pages;

namespace Anastasia423WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для PartSelectionPage.xaml
    /// </summary>
    public partial class PartSelectionPage : Page
    {
        private int _partTypeId;

        public PartSelectionPage(int partTypeId)
        {
            InitializeComponent();
            _partTypeId = partTypeId;
            LoadFilters();
            ApplyFilters();
        }

        private void LoadFilters()
        {
            var manufacturers = Core.Context.manufacturer_.ToList();
            manufacturers.Insert(0, new manufacturer_ { id = 0, name = "Все производители" });
            CmbManufacturers.ItemsSource = manufacturers;
            CmbManufacturers.SelectedIndex = 0;
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            // Подгружаем производителя
            var query = Core.Context.basepart_.Include("manufacturer_");

            // Подгружаем характеристики в зависимости от открытой категории.
            // Строки в кавычках должны строго совпадать с названиями свойств в классе basepart_!
            switch (_partTypeId)
            {
                case 1: query = query.Include("cpu_"); break;
                case 2: query = query.Include("gpu_"); break;
                case 3: query = query.Include("ram_"); break;
                case 6: query = query.Include("powersupply_"); break;
            }

            var list = query.Where(p => p.parttypeid == _partTypeId).ToList();

            // 3. Фильтрация по поисковой строке (название детали)
            if (!string.IsNullOrWhiteSpace(TxtSearch.Text))
            {
                list = list.Where(p => p.name.ToLower().Contains(TxtSearch.Text.ToLower())).ToList();
            }

            // 4. Фильтрация по производителю из ComboBox
            if (CmbManufacturers.SelectedItem is manufacturer_ selectedMan && selectedMan.id != 0)
            {
                list = list.Where(p => p.manufacturerid == selectedMan.id).ToList();
            }

            // 5. ЛОГИКА СОВМЕСТИМОСТИ
            // Перед фильтрацией получаем данные уже выбранных компонентов, чтобы использовать их как константы

            // Совместимость сокета (Процессор <-> Мат.плата)
            if (_partTypeId == 1 && BuildManager.Motherboard != null) // Выбираем CPU
            {
                var selectedMobo = Core.Context.motherboard_.Find(BuildManager.Motherboard.id);
                list = list.Where(p => p.cpu_.socketid == selectedMobo.socketid).ToList();
            }
            else if (_partTypeId == 4 && BuildManager.CPU != null) // Выбираем материнку
            {
                var selectedCpu = Core.Context.cpu_.Find(BuildManager.CPU.id);
                list = list.Where(p => p.motherboard_.socketid == selectedCpu.socketid).ToList();
            }

            // Совместимость типа памяти (Мат.плата <-> ОЗУ)
            if (_partTypeId == 3 && BuildManager.Motherboard != null) // Выбираем RAM
            {
                var selectedMobo = Core.Context.motherboard_.Find(BuildManager.Motherboard.id);
                list = list.Where(p => p.ram_.memorytypeid == selectedMobo.memorytypeid).ToList();
            }

            // Совместимость Блока питания (БП <-> Видеокарта)
            if (_partTypeId == 6 && BuildManager.GPU != null) // Выбираем Блок питания
            {
                var selectedGpu = Core.Context.gpu_.Find(BuildManager.GPU.id);
                // Оставляем только те БП, мощность которых >= рекомендованной для видеокарты
                list = list.Where(p => p.powersupply_.power >= selectedGpu.recommendpower).ToList();
            }

            // Совместимость корпуса по форм-фактору мат.платы
            if (_partTypeId == 5 && BuildManager.Motherboard != null) // Выбираем Корпус
            {
                var selectedMobo = Core.Context.motherboard_.Find(BuildManager.Motherboard.id);
                // Проверяем через связующую таблицу, подходит ли форм-фактор платы к этому корпусу
                list = list.Where(p => Core.Context.boardformfactorcase_
                    .Any(bfc => bfc.caseid == p.id && bfc.formfactorid == selectedMobo.formfactorid)).ToList();
            }

            // 6. Выводим итоговый отфильтрованный список на экран
            LwParts.ItemsSource = list;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is basepart_ selectedPart)
            {
                switch (_partTypeId)
                {
                    case 1: BuildManager.CPU = selectedPart; break;
                    case 2: BuildManager.GPU = selectedPart; break;
                    case 3: BuildManager.RAM = selectedPart; break;
                    case 4: BuildManager.Motherboard = selectedPart; break;
                    case 5: BuildManager.Case = selectedPart; break;
                    case 6: BuildManager.PowerSupply = selectedPart; break;
                    case 7: BuildManager.Cooler = selectedPart; break;
                    case 8: BuildManager.Storage = selectedPart; break;
                }
                NavigationService.GoBack();
            }
        }
    }
}
