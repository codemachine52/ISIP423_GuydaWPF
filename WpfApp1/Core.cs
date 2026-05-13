using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class Core
    {
        private static MG_KISHEntities1 _context;

        public static MG_KISHEntities1 Context
        {
            get
            {
                // Реализация Singleton для экономии ресурсов
                if (_context == null)
                    _context = new MG_KISHEntities1();
                return _context;
            }

        }
        public static user_ CurrentUser { get; set; }

        /// <summary>
        /// Метод для проверки доступности базы данных
        /// </summary>
        public static bool CheckConnection()
        {
            try
            {
                return Context.Database.Exists();
            }
            catch
            {
                return false;
            }
        }
    }
}
