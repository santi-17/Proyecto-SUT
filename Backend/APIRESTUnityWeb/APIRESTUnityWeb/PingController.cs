using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace APIRESTUnityWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult get()
        {
            return Ok("Pong");
        }
    }
}
