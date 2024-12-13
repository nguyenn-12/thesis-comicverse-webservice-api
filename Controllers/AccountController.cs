using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using thesis_comicverse_webservice_api.DTOs.AuthenticationDTO;
using thesis_comicverse_webservice_api.Repositories;

namespace thesis_comicverse_webservice_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, IUserRepository userRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpGet, Authorize]
        [Route("get-me")]
        public ActionResult<string> GetMe()
        {
            var userName = _userRepository.GetMyName();
            return Ok(userName);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registInfor)
        {
            try
            {
                _logger.LogInformation("Register a user");

                var registeredUser = await _userRepository.Register(registInfor);

                return Ok(registeredUser);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginInfor)
        {
            try
            {
                _logger.LogInformation("Logging in user");

                var loggedInUser = await _userRepository.Login(loginInfor);

                return Ok(loggedInUser);
            }
            catch
            {
                throw;
            }
        }

    }
}
