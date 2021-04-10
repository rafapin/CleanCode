using CleanCode.Models;
using CleanCode.Services.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly IdbRoulettes _db;
        public RouletteController(IdbRoulettes db)
        {
            _db = db;
        }

        [HttpPost("Create")]
        public IActionResult Create()
        {
            try
            {
                return Ok(_db.CreateRoulette());
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Bet")]
        public IActionResult Bet(BetRoulette model)
        {
            try
            {
                string msg = "";
                model.IdClient = Convert.ToInt32(Request.Headers["IdClient"]);
                if (model.validate())
                {
                    _db.CreateBet(model);
                    msg = "Se ha creado la apuesta correctamente";
                }
                else msg = "Se tiene que hacer una apuesta válida";
                
                return Ok(msg);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
