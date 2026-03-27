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
            var query = Core.Context.basepart_.Where(p => p.parttypeid == _partTypeId).ToList();

            // Поиск
            if (!string.IsNullOrWhiteSpace(TxtSearch.Text))
            {
                query = query.Where(p => p.name.ToLower().Contains(TxtSearch.Text.ToLower())).ToList();
            }

            // Фильтрация по производителю
            if (CmbManufacturers.SelectedItem is manufacturer_ selectedMan && selectedMan.id != 0)
            {
                query = query.Where(p => p.manufacturerid == selectedMan.id).ToList();
            }

            // ПРОВЕРКИ НА СОВМЕСТИМОСТЬ (ОСТАВЛЯЕМ ТОЛЬКО ПОДХОДЯЩИЕ)

            // 1. Совместимость сокета CPU и Материнской платы
            if (_partTypeId == 1 && BuildManager.Motherboard != null) // Выбираем CPU
            {
                var moboSocket = Core.Context.motherboard_.FirstOrDefault(m => m.id == BuildManager.Motherboard.id)?.socketid;
                query = query.Where(p => Core.Context.cpu_.FirstOrDefault(c => c.id == p.id)?.socketid == moboSocket).ToList();
            }
            if (_partTypeId == 4 && BuildManager.CPU != null) // Выбираем Mobo
            {
                var cpuSocket = Core.Context.cpu_.FirstOrDefault(c => c.id == BuildManager.CPU.id)?.socketid;
                query = query.Where(p => Core.Context.motherboard_.FirstOrDefault(m => m.id == p.id)?.socketid == cpuSocket).ToList();
            }

            // 2. Совместимость сокета Кулера
            if (_partTypeId == 7) // Выбираем Кулер
            {
                int? socketId = null;
                if (BuildManager.CPU != null)
                    socketId = Core.Context.cpu_.FirstOrDefault(c => c.id == BuildManager.CPU.id)?.socketid;
                else if (BuildManager.Motherboard != null)
                    socketId = Core.Context.motherboard_.FirstOrDefault(m => m.id == BuildManager.Motherboard.id)?.socketid;

                if (socketId.HasValue)
                {
                    query = query.Where(p => Core.Context.socketprocessorcooler_.Any(spc => spc.processorcoolerid == p.id && spc.socketid == socketId.Value)).ToList();
                }
            }

            // 3. Совместимость Форм-фактора Материнской платы и Корпуса
            if (_partTypeId == 5 && BuildManager.Motherboard != null) // Выбираем Корпус
            {
                var moboFormFactor = Core.Context.motherboard_.FirstOrDefault(m => m.id == BuildManager.Motherboard.id)?.formfactorid;
                query = query.Where(p => Core.Context.boardformfactorcase_.Any(bfc => bfc.caseid == p.id && bfc.formfactorid == moboFormFactor)).ToList();
            }

            // 4. Совместимость типа памяти (Mobo и RAM)
            if (_partTypeId == 3 && BuildManager.Motherboard != null) // Выбираем оперативку
            {
                var moboMemoryType = Core.Context.motherboard_.FirstOrDefault(m => m.id == BuildManager.Motherboard.id)?.memorytypeid;
                query = query.Where(p => Core.Context.ram_.FirstOrDefault(r => r.id == p.id)?.memorytypeid == moboMemoryType).ToList();
            }

            // 5. Мощность блока питания и GPU (БП должен быть >= рекомендуемой мощности видеокарты как минимум
            if (_partTypeId == 6 && BuildManager.GPU != null) // Выбираем БП
            {
                var gpuRecPower = Core.Context.gpu_.FirstOrDefault(g => g.id == BuildManager.GPU.id)?.recommendpower ?? 0;
                query = query.Where(p => Core.Context.powersupply_.FirstOrDefault(ps => ps.id == p.id)?.power >= gpuRecPower).ToList();
            }

            LwParts.ItemsSource = query;
        }

        private void BtnSelectPart_Click(object sender, RoutedEventArgs e)
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

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
