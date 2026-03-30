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
            // 1. Начинаем с базового запроса. 
            // Используем .Include("Manufacturer"), чтобы получить данные производителя для XAML.
            var query = Core.Context.basepart_
                .Include("manufacturer_")
                .Where(p => p.parttypeid == _partTypeId)
                .ToList();

            // 2. Поиск по названию
            if (!string.IsNullOrWhiteSpace(TxtSearch.Text))
            {
                query = query.Where(p => p.name.ToLower().Contains(TxtSearch.Text.ToLower())).ToList();
            }

            // 3. Фильтрация по производителю (из ComboBox)
            if (CmbManufacturers.SelectedItem is manufacturer_ selectedMan && selectedMan.id != 0)
            {
                query = query.Where(p => p.manufacturerid == selectedMan.id).ToList();
            }

            // 4. ПРОВЕРКИ НА СОВМЕСТИМОСТЬ
            // Эти проверки ограничивают список доступных деталей в зависимости от того, что уже выбрано.

            // Совместимость сокета (Процессор <-> Материнская плата)
            if (_partTypeId == 1 && BuildManager.Motherboard != null) // Выбираем процессор
            {
                var moboSocketId = Core.Context.motherboard_.Find(BuildManager.Motherboard.id).socketid;
                query = query.Where(p => Core.Context.cpu_.Find(p.id).socketid == moboSocketId).ToList();
            }
            else if (_partTypeId == 4 && BuildManager.CPU != null) // Выбираем мат. плату
            {
                var cpuSocketId = Core.Context.cpu_.Find(BuildManager.CPU.id).socketid;
                query = query.Where(p => Core.Context.motherboard_.Find(p.id).socketid == cpuSocketId).ToList();
            }

            // Совместимость типа памяти (Материнская плата <-> Оперативная память)
            if (_partTypeId == 3 && BuildManager.Motherboard != null) // Выбираем RAM
            {
                var moboMemoryTypeId = Core.Context.motherboard_.Find(BuildManager.Motherboard.id).memorytypeid;
                query = query.Where(p => Core.Context.ram_.Find(p.id).memorytypeid == moboMemoryTypeId).ToList();
            }

            // Совместимость форм-фактора (Материнская плата <-> Корпус)
            if (_partTypeId == 5 && BuildManager.Motherboard != null) // Выбираем Корпус
            {
                var moboFormFactorId = Core.Context.motherboard_.Find(BuildManager.Motherboard.id).formfactorid;
                // Проверяем через связующую таблицу boardformfactorcase
                query = query.Where(p => Core.Context.boardformfactorcase_
                    .Any(bfc => bfc.caseid == p.id && bfc.formfactorid == moboFormFactorId)).ToList();
            }

            // Совместимость по питанию (Блок питания <-> Видеокарта)
            if (_partTypeId == 6 && BuildManager.GPU != null) // Выбираем Блок питания
            {
                var gpuPowerReq = Core.Context.gpu_.Find(BuildManager.GPU.id).recommendpower;
                query = query.Where(p => Core.Context.powersupply_.Find(p.id).power >= gpuPowerReq).ToList();
            }

            // Совместимость кулера по сокету
            if (_partTypeId == 7 && (BuildManager.CPU != null || BuildManager.Motherboard != null))
            {
                int? targetSocket = BuildManager.CPU != null
                    ? Core.Context.cpu_.Find(BuildManager.CPU.id).socketid
                    : Core.Context.motherboard_.Find(BuildManager.Motherboard.id).socketid;

                query = query.Where(p => Core.Context.socketprocessorcooler_
                    .Any(spc => spc.processorcoolerid == p.id && spc.socketid == targetSocket)).ToList();
            }

            // 5. Отправляем итоговый список в ListView
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
