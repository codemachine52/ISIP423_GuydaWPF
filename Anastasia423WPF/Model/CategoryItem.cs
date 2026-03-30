using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anastasia423WPF.Model
{
    public class CategoryItem
    {
        public string Title { get; set; }
        public int PartTypeId { get; set; }
        public basepart_ SelectedPart { get; set; }
        
        // Свойство для удобного отображения цены или текста "Не выбрано"
        public string DisplayPrice => SelectedPart != null ? $"{SelectedPart.price:N0} руб." : "";
        public string DisplayName => SelectedPart != null ? SelectedPart.name : "Не выбрано";
    }
}
