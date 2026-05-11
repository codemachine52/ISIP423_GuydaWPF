using System.Linq;
using System.Windows;

namespace WpfApp1
{
    public partial class book
    {
        // Теперь точно правильно: вытаскиваем имена через связь
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
        public Visibility AdminVisibility => (Core.CurrentUser?.RoleID == 1) ? Visibility.Visible : Visibility.Collapsed;
    }
}