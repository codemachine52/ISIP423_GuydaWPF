using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class ViewModel
    {
        public Film Film { get; set; }
        public List<SessionDisplay> sessions { get; set; }
    }
}
