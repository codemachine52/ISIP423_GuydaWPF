using System.Collections.Generic;
using System.Linq;
using Anastasia423WPF;

namespace Anastasia423WPF
{
    public static class BuildManager
    {
        public static basepart_ CPU { get; set; }
        public static basepart_ Motherboard { get; set; }
        public static basepart_ RAM { get; set; }
        public static basepart_ GPU { get; set; }
        public static basepart_ Case { get; set; }
        public static basepart_ PowerSupply { get; set; }
        public static basepart_ Cooler { get; set; }
        public static basepart_ Storage { get; set; }

        public static decimal TotalPrice => GetSelectedParts().Sum(p => p.price);

        public static List<basepart_> GetSelectedParts()
        {
            var parts = new List<basepart_> { CPU, Motherboard, RAM, GPU, Case, PowerSupply, Cooler, Storage };
            return parts.Where(p => p != null).ToList();
        }

        public static void Clear()
        {
            CPU = Motherboard = RAM = GPU = Case = PowerSupply = Cooler = Storage = null;
        }
    }
}