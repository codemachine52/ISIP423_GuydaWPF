using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Anastasia423WPF.Pages;
using Anastasia423WPF.Model;

namespace Anastasia423WPF.Pages
{
    public partial class CurrentBuildPage : Page
    {
        public CurrentBuildPage()
        {
            InitializeComponent();
        }

        private void CurrentBuildPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Формируем список данных комплектующих
            var items = new List<CategoryItem>
            {
                new CategoryItem { Title = "Процессор (CPU)", PartTypeId = 1, SelectedPart = BuildManager.CPU },
                new CategoryItem { Title = "Материнская плата", PartTypeId = 4, SelectedPart = BuildManager.Motherboard },
                new CategoryItem { Title = "Видеокарта (GPU)", PartTypeId = 2, SelectedPart = BuildManager.GPU },
                new CategoryItem { Title = "Оперативная память", PartTypeId = 3, SelectedPart = BuildManager.RAM },
                new CategoryItem { Title = "Кулер процессора", PartTypeId = 7, SelectedPart = BuildManager.Cooler },
                new CategoryItem { Title = "Блок питания", PartTypeId = 6, SelectedPart = BuildManager.PowerSupply },
                new CategoryItem { Title = "Накопитель", PartTypeId = 8, SelectedPart = BuildManager.Storage },
                new CategoryItem { Title = "Корпус", PartTypeId = 5, SelectedPart = BuildManager.Case }
            };

            // Передаем данные в XAML
            CategoriesItemsControl.ItemsSource = items;

            // Обновляем общую цену
            TxtTotalPrice.Text = $"{BuildManager.TotalPrice:N0} руб.";
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            // Кнопка узнает, к какой категории она относится, через свойство Tag
            if (sender is Button btn && btn.Tag is CategoryItem item)
            {
                NavigationService.Navigate(new PartSelectionPage(item.PartTypeId));
            }
        }

        private void BtnSaveBuild_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBuildName.Text) || string.IsNullOrWhiteSpace(TxtAuthorName.Text))
            {
                MessageBox.Show("Заполните название и имя автора!");
                return;
            }

            var parts = BuildManager.GetSelectedParts();
            if (parts.Count == 0)
            {
                MessageBox.Show("Сборка пуста!");
                return;
            }

            var newAssembly = new assembly_
            {
                name = TxtBuildName.Text,
                author = TxtAuthorName.Text
            };

            Core.Context.assembly_.Add(newAssembly);
            Core.Context.SaveChanges();

            foreach (var part in parts)
            {
                Core.Context.partassembly_.Add(new partassembly_
                {
                    assemblyid = newAssembly.id,
                    partid = part.id
                });
            }
            Core.Context.SaveChanges();

            MessageBox.Show("Сборка успешно сохранена!");
            BuildManager.Clear();
            TxtBuildName.Clear();
            TxtAuthorName.Clear();
            UpdateUI();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            BuildManager.Clear();
            UpdateUI();
        }

        private void BtnViewSaved_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SavedBuildsPage());
        }
    }
}