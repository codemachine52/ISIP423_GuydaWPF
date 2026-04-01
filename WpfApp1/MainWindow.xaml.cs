using System;
using System.Windows;

namespace SalaryCalculator
{
    public partial class MainWindow : Window
    {
        // Инициализируем сервис для расчетов
        private readonly SalaryService _salaryService = new SalaryService();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Валидация ввода (Краткое изложение теста) 
            if (!double.TryParse(HoursTextBox.Text, out double hours))
            {
                MessageBox.Show("Введите числовое значение в поле 'Количество часов'.", "Ошибка");
                return;
            }

            try
            {
                // 2. Определение выбранной должности
                Position selectedPosition = Position.Assistant;
                if (DocentRadio.IsChecked == true) selectedPosition = Position.Docent;
                if (ProfessorRadio.IsChecked == true) selectedPosition = Position.Professor;

                // 3. Вызов метода расчета (Бизнес-логика)
                bool isTaxEnabled = TaxCheckBox.IsChecked ?? false;
                var results = _salaryService.Calculate(hours, selectedPosition, isTaxEnabled);

                // 4. Отображение Фактического результата 
                AccruedLabel.Content = "Начислено: " + results.GrossSalary + " руб.";
                TaxLabel.Content = "в том числе налог: " + results.TaxAmount + " руб.";
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка данных");
            }
        }
    }
}