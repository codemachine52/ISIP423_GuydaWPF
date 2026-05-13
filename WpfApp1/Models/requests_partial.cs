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
        public string TargetName
        {
            get
            {
                // Если в БД bookID не null, значит это заявка на книгу
                if (bookID != null)
                {
                    // Если book подгрузился через Include, выводим имя, иначе - ID
                    return book != null ? $"Книга: {book.Name}" : $"Книга (ID: {bookID})";
                }
                // Иначе это заявка на пользователя
                return user_ != null ? $"Аккаунт: {user_.Login}" : "Неизвестный объект";
            }
        }
    }

    // Для заявок на роль
    public partial class requestRole
    {
        public string UserName => user_ != null ? user_.Login : "Нет данных";
    }
}
