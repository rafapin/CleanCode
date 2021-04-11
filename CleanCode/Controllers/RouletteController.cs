using CleanCode.Models;
using CleanCode.Services.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanCode.Extensions;
using Microsoft.Extensions.Logging;

namespace CleanCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly IdbRoulettes _db;
        private readonly IDistributedCache _cache;
        private readonly ILogger _log;
        public RouletteController(IdbRoulettes db, IDistributedCache cache, ILogger log)
        {
            _db = db;
            _cache = cache;
            _log = log;
        }

        [HttpPost("Create")]
        public IActionResult Create()
        {
            try
            {
                _log.LogInformation("Creating Roulette...");
                return Ok(_db.CreateRoulette());
            }
            catch(Exception ex)
            {
                _log.LogWarning("Error: " + ex.Message);
                return BadRequest(ex);
            }
        }

        [HttpGet("VerifyStatus")]
        public IActionResult VerifyStatus(int Id)
        {
            try
            {
                string msg;
                _log.LogInformation("Verify Status of Roulette Id: "+Id.ToString());
                if (_db.VerifyStatus(Id)) msg = "La operación fue existosa";
                else msg = "La operación fue denegada";
                _log.LogInformation("Already: " + msg);
                return Ok(msg);
            }
            catch(Exception ex)
            {
                _log.LogWarning("Error: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Bet")]
        public IActionResult Bet(BetRoulette model)
        {

            _log.LogInformation("Validating Model...");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                string msg = "";
                model.IdClient = Convert.ToInt32(Request.Headers["IdClient"]);
                if (model.validate())
                {
                    _log.LogInformation("Model is Ok...");
                    _db.CreateBet(model);
                    msg = "Se ha creado la apuesta correctamente";
                }
                else
                {
                    msg = "Se tiene que hacer una apuesta válida";
                    _log.LogInformation("Model is Invalid...");
                }
                _log.LogInformation("The bet has been created successfully !");

                return Ok(msg);
            }
            catch(Exception ex)
            {
                _log.LogWarning("Error: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Close/{Id}")]
        public IActionResult Close(int Id)
        {
            try
            {
                _log.LogInformation("Getting the bets...");
                var InBets = _db.ListBets(Id);
                CalculateCloseRoulette calculate = new CalculateCloseRoulette();
                _log.LogInformation("Calculating de Amount Earned...");
                var OutBets = calculate.Calculate(InBets);
                ResponseBets model = new ResponseBets();
                model.Bets = OutBets;
                model.IdRoulette = Id;
                model.WinNumber = calculate.WinNumber;
                _log.LogInformation("Updating Roulette...");
                _db.UpdateRoulette(model);
                _log.LogInformation("Everything has been successful !");
                return Ok(model);
            }
            catch(Exception ex)
            {
                _log.LogWarning("Error: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            try
            {
                _log.LogInformation("Validating Cache...");
                string recordKey = "ListRoulettes_" + DateTime.Now.ToString("yyyyMMdd_hhmmss");
                List<Roulette> InRoulettes = await _cache.GetRecordAsync<List<Roulette>>(recordKey);
                if(InRoulettes is null)
                {
                    _log.LogInformation("Getting Data of Database...");
                    InRoulettes = _db.GetRoulettes();
                    await _cache.SetRecordAsync(recordKey, InRoulettes);
                }
                _log.LogInformation("Fixing Answer...");
                var OutRoulettes = new ResponseListRoulettes(InRoulettes);
                _log.LogInformation("Everything has been successful !");
                return Ok(OutRoulettes);
            }
            catch (Exception ex)
            {
                _log.LogWarning("Error: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
