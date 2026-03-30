using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Anastasia423WPF;
using Anastasia423WPF.Model;
using Anastasia423WPF.Pages;

namespace BuilderPC
{
    public partial class CurrentBuildPage : Page
    {
        public CurrentBuildPage()
        {
            InitializeComponent();
        }

        // Этот метод будет срабатывать ВСЕГДА, когда вы возвращаетесь на эту страницу
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            var items = new List<CategoryItem>
        {
            new CategoryItem { Title = "ПРОЦЕССОР", PartTypeId = 1, SelectedPart = BuildManager.CPU },
            new CategoryItem { Title = "МАТЕРИНСКАЯ ПЛАТА", PartTypeId = 4, SelectedPart = BuildManager.Motherboard },
            new CategoryItem { Title = "ВИДЕОКАРТА", PartTypeId = 2, SelectedPart = BuildManager.GPU },
            new CategoryItem { Title = "ОПЕРАТИВНАЯ ПАМЯТЬ", PartTypeId = 3, SelectedPart = BuildManager.RAM },
            new CategoryItem { Title = "БЛОК ПИТАНИЯ", PartTypeId = 6, SelectedPart = BuildManager.PowerSupply },
            new CategoryItem { Title = "КОРПУС", PartTypeId = 5, SelectedPart = BuildManager.Case }
        };

            CategoriesItems.ItemsSource = items;
            TxtTotalPrice.Text = $"{BuildManager.TotalPrice:N0} руб.";
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Tag is CategoryItem item)
                NavigationService.Navigate(new PartSelectionPage(item.PartTypeId));
        }

        private void BtnSaveBuild_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBuildName.Text)) { MessageBox.Show("Введите название!"); return; }

            var newBuild = new assembly_ { name = TxtBuildName.Text, author = TxtAuthorName.Text };
            Core.Context.assembly_.Add(newBuild);
            Core.Context.SaveChanges();

            foreach (var p in BuildManager.GetSelectedParts())
                Core.Context.partassembly_.Add(new partassembly_ { assemblyid = newBuild.id, partid = p.id });

            Core.Context.SaveChanges();
            MessageBox.Show("Сборка сохранена!");
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e) { BuildManager.Clear(); UpdateUI(); }
        private void BtnViewSaved_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new SavedBuildsPage());
    }
}