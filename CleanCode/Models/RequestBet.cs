using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanCode.Models
{
    public class RequestBet
    {
        public int IdBet { get; set; }
        public double AmountEarned { get; set; }
        public int IdClient { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
