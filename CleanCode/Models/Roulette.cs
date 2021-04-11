using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanCode.Models
{
    public class Roulette
    {
        public int IdRoulette { get; set; }
        public int? WinNumber { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateClose { get; set; }
    }
}
