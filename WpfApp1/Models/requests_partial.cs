using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public partial class requestUnFreeze
    {
        public string TargetName => bookID == null ? $"Аккаунт: {user_.Login}" : $"Книга: {book.Name}";
    }

    // Для заявок на роль
    public partial class requestRole
    {
        public string UserName => user_.Login;
    }
}
