using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class user_
    {
        public string StatusText => IsFreeze ? "Заблокирован" : "Активен";

        // Свойство для цвета текста статуса
        public SolidColorBrush StatusColor => IsFreeze ? Brushes.Red : Brushes.Green;

        // Свойство для текста на кнопке (чтобы понимать, что она сделает)
        public string ActionButtonText => IsFreeze ? "Разблокировать" : "Заморозить";
    }
}
