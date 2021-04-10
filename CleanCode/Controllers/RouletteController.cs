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
    }
}
