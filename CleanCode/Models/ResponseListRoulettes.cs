using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanCode.Models
{
    public class ResponseListRoulettes
    {
        public ResponseListRoulettes(List<Roulette> roulettes)
        {
            ListRoulettes = Match(roulettes);
        }

        public List<OutRoulette> ListRoulettes { get; set; }

        private List<OutRoulette> Match(List<Roulette> roulettes)
        {
            var OutRoulettes = new List<OutRoulette>();
            foreach(var roulette in roulettes)
            {
                var item = new OutRoulette();
                item.IdRoulette = roulette.IdRoulette;
                if (roulette.WinNumber == null) item.Status = "Abierta";
                else item.Status = "Cerrada";
                OutRoulettes.Add(item);
            }
            return OutRoulettes;
        }

    }
}
