using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anastasia423WPF
{
    public partial class Product
    {
        public bool HasBigDiscount
        {
            get
            {
                return Discount > 15;
            }
        }
    }
}
