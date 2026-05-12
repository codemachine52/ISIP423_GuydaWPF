using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class book
    {
        public string GenresDisplay
        {
            get
            {
                // Заходим в таблицу связей, фильтруем по текущей книге
                // и через навигационное свойство .ganre достаем .Name
                var genreNames = Core.Context.BookGanre
                                     .Where(bg => bg.BookID == this.ID)
                                     .Select(bg => bg.ganre.Name)
                                     .ToList();

                return genreNames.Count > 0
                    ? string.Join(", ", genreNames)
                    : "Жанры не указаны";
            }
        }

        // Логика для админской кнопки заморозки
        public string FreezeActionText => (IsFreeze == true) ? "Разморозить книгу" : "Заморозить книгу";

        // Видимость админ-панелей (используем глобального Core.CurrentUser)
        public Visibility AdminVisibility => (Core.CurrentUser?.RoleID == 3) ? Visibility.Visible : Visibility.Collapsed;
        // Текст статуса
        public string StatusText => IsFreeze == true ? "Заморожена" : "Опубликована";

        // Цвет статуса
        public SolidColorBrush StatusColor => IsFreeze == true
            ? new SolidColorBrush(Color.FromRgb(255, 76, 76))
            : new SolidColorBrush(Color.FromRgb(39, 166, 175));

        // Видимость кнопки "Оспорить" (только если заморожена)
        public Visibility AppealVisibility => IsFreeze == true ? Visibility.Visible : Visibility.Collapsed;
    }
}