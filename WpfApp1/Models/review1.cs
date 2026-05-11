using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public partial class review
    {
        // Теперь у каждого отзыва есть знание о том, видна ли админка
        public Visibility AdminVisibility => (Core.CurrentUser?.RoleID == 3) ? Visibility.Visible : Visibility.Collapsed;

        // Текст для кнопки заморозки отзыва
        public string FreezeActionText => (this.IsFreeze == true) ? "Разморозить" : "Заморозить";
    }
}
