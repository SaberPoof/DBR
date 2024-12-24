using DBRobot.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DBRobot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DBRController : ControllerBase
    {
        private readonly PgContext _context;

        public DBRController(PgContext pgContext)
        {
            _context = pgContext;
        }

        [HttpGet]
        public IActionResult GetDataFromDB()
        {
            var data = _context.dbData.ToList();
            if (data.Any())
            {
                return Ok(data);
            }
            else
            {
                return BadRequest($"{data}: данных нет");
            }
        }
    }
}
