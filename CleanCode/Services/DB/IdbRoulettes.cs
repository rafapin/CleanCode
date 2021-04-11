using CleanCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanCode.Services.DB
{
    public interface IdbRoulettes
    {
        int CreateRoulette();
        void CreateBet(BetRoulette model);
        bool VerifyStatus(int id);
    }
}
