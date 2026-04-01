using System;

namespace SalaryCalculator
{
    // Перечисление должностей на основе полей из ТЗ
    public enum Position
    {
        Assistant,
        Docent,
        Professor
    }

    public class SalaryService
    {
        public (double GrossSalary, double TaxAmount) Calculate(double hours, Position position, bool includeTax)
        {
            if (hours < 0)
            {
                throw new ArgumentException("Количество часов не может быть отрицательным.");
            }

            double rate;

            // Используем классический switch для совместимости с C# 7.3
            switch (position)
            {
                case Position.Assistant:
                    rate = 150; // Ставка ассистента
                    break;
                case Position.Docent:
                    rate = 250; // Ставка доцента
                    break;
                case Position.Professor:
                    rate = 350; // Ставка профессора
                    break;
                default:
                    rate = 0;
                    break;
            }

            double gross = hours * rate;
            double tax = 0;

            // Расчет налога 13%, если выбрано пользователем
            if (includeTax)
            {
                tax = gross * 0.13;
            }

            return (Math.Round(gross, 2), Math.Round(tax, 2));
        }
    }
}