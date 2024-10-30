using Employee_Management.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Employee_Management.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeApiController : ControllerBase
    {
        private readonly ILogger<HomeApiController> _logger;

        public HomeApiController(ILogger<HomeApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            return Ok("Index action of HomeApiController");
        }

        [HttpGet("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return Ok(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
