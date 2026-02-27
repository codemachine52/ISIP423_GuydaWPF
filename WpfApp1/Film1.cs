using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public partial class Film
    {
        public int RatingColorCode
        {
            get
            {
                if (Rating >= 7) return 1;
                if (Rating >= 6 && Rating < 7) return 2;
                if (Rating >= 4 && Rating < 6) return 3;
                return 4;
            }
        }
    }
}
