using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanCode.Models
{
    public class BetRoulette
    {
        public int IdBet { get; set; }
        public int? Number { get; set; }
        public string Color { get; set; }
        private bool status = false;
        public double Amount { get; set; }
        public int IdClient { get; set; }
        

        public bool validate()
        {
            if (Number == null && Color == null) status = false;
            else if(Number != null && Color != null) status = false;
            else
            {
                if (Number == null)
                {
                    foreach (var color in Colors)
                    {
                        if (Color == color) status = true;
                    }
                }
                else
                {
                    if (Number >= 0 && Number <= 36) status = true;
                }
            }

            return status;
        }

        public List<string> Colors = new List<string> {"black","red"};

    }
}
