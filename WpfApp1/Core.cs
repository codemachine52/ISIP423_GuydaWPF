using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class Core
    {
        private static MG_KISHEntities _context;

        public static MG_KISHEntities Context
        {
            get
            {
                // Реализация Singleton для экономии ресурсов
                if (_context == null)
                    _context = new MG_KISHEntities();
                return _context;
            }
        }

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
