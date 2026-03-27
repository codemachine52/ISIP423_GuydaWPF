using System.Windows;
using System.Windows.Controls;
using Anastasia423WPF;
using Anastasia423WPF.Pages;

namespace BuilderPC
{
    public partial class CurrentBuildPage : Page
    {
        public CurrentBuildPage()
        {
            InitializeComponent();
            Loaded += CurrentBuildPage_Loaded;
        }

        private void CurrentBuildPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            CategoriesPanel.Children.Clear();
            AddCategoryRow("Процессор (CPU)", 1, BuildManager.CPU);
            AddCategoryRow("Материнская плата", 4, BuildManager.Motherboard);
            AddCategoryRow("Видеокарта (GPU)", 2, BuildManager.GPU);
            AddCategoryRow("Оперативная память", 3, BuildManager.RAM);
            AddCategoryRow("Кулер процессора", 7, BuildManager.Cooler);
            AddCategoryRow("Блок питания", 6, BuildManager.PowerSupply);
            AddCategoryRow("Накопитель", 8, BuildManager.Storage);
            AddCategoryRow("Корпус", 5, BuildManager.Case);

            TxtTotalPrice.Text = $"{BuildManager.TotalPrice:C2}";
        }

        private void AddCategoryRow(string title, int partTypeId, basepart_ selectedPart)
        {
            var border = new Border { BorderBrush = System.Windows.Media.Brushes.LightGray, BorderThickness = new Thickness(1), Margin = new Thickness(0, 0, 0, 10), Padding = new Thickness(10) };
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var infoStack = new StackPanel();
            infoStack.Children.Add(new TextBlock { Text = title, FontWeight = FontWeights.Bold });

            if (selectedPart != null)
            {
                infoStack.Children.Add(new TextBlock { Text = selectedPart.name, FontSize = 16, Margin = new Thickness(0, 5, 0, 0) });
                infoStack.Children.Add(new TextBlock { Text = $"{selectedPart.price:C2}", Foreground = System.Windows.Media.Brushes.DarkGreen });
            }
            else
            {
                infoStack.Children.Add(new TextBlock { Text = "Не выбрано", FontStyle = FontStyles.Italic, Foreground = System.Windows.Media.Brushes.Gray, Margin = new Thickness(0, 5, 0, 0) });
            }

            var btn = new Button { Content = selectedPart == null ? "Выбрать" : "Заменить", Padding = new Thickness(15, 5, 15, 5), VerticalAlignment = VerticalAlignment.Center };
            btn.Click += (s, e) => NavigationService.Navigate(new PartSelectionPage(partTypeId));

            Grid.SetColumn(infoStack, 0);
            Grid.SetColumn(btn, 1);
            grid.Children.Add(infoStack);
            grid.Children.Add(btn);
            border.Child = grid;

            CategoriesPanel.Children.Add(border);
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
            Core.Context.SaveChanges(); // Сохраняем, чтобы получить ID

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