using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Anastasia423WPF.Model
{
    public class CategoryItem
    {
        public string Title { get; set; }
        public int PartTypeId { get; set; }
        public basepart_ SelectedPart { get; set; }

        // Логика отображения полей
        public string DisplayName => SelectedPart?.name ?? "Не выбрано";
        public string DisplayPrice => SelectedPart != null ? $"{SelectedPart.price:N0} руб." : "";
        public Visibility PriceVisibility => SelectedPart != null ? Visibility.Visible : Visibility.Collapsed;
    }
}
