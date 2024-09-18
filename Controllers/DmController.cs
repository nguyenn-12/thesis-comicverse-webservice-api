using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thesis_comicverse_webservice_api.Models;

namespace thesis_comicverse_webservice_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DmController : ControllerBase
    {
        [HttpGet(Name = "DmTest")]
        public IActionResult Get()
        {
            TestDm DucManh = new TestDm
            {
                Name = "John Doe",
                Age = 25
            };
            return Ok(DucManh);
        }
    }
}
