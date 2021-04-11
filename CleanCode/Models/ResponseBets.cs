using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanCode.Models
{
    public class ResponseBets
    {
        public int IdRoulette { get; set; }
        public int WinNumber { get; set; }
        public List<RequestBet> Bets { get; set; }
    }
}
