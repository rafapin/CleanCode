using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanCode.Models
{
    public class CalculateCloseRoulette
    {

        public readonly int WinNumber;

        public CalculateCloseRoulette()
        {
            WinNumber = GenerateNumber();
        }

        private int GenerateNumber()
        {
            var rnd = new Random();
            return rnd.Next(0, 36);
        }

        public List<RequestBet> Calculate(List<BetRoulette> InBets)
        {
            List<RequestBet> OutBets = new List<RequestBet>();
            foreach(var bet in InBets)
            {
                double AmountEarned;
                if (bet.Number != null) AmountEarned = WinForNumber(bet);
                else AmountEarned = WinForColor(bet);
                OutBets.Add(new RequestBet
                {
                    IdBet = bet.IdBet,
                    IdClient = bet.IdClient,
                    AmountEarned = AmountEarned
                });
            }

            return OutBets;
        }

        private double WinForNumber(BetRoulette bet)
        {
            if (bet.Number == WinNumber) return 5 * bet.Amount;
            else return 0;
        }

        private double WinForColor(BetRoulette bet)
        {
            int ColorToNumber;
            if (bet.Color.ToLower() == "red") ColorToNumber = 0;
            else if (bet.Color.ToLower() == "black") ColorToNumber = 1;
            else return 0;

            if (ColorToNumber == WinNumber%2) return 1.8 * bet.Amount;
            else return 0;
        }
    }
}
