using Microsoft.AspNetCore.Mvc;
using thesis_comicverse_webservice_api.Models;
using thesis_comicverse_webservice_api.Repositories;

namespace thesis_comicverse_webservice_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpGet(Name = "GetUser")]
        public IActionResult GetAllUsers()
        {
            _logger.LogInformation("Getting all users");
            var users = _userRepository.GetAllUsers();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public IActionResult GetUserByUserID(int id)
        {
            var user = _userRepository.GetUserByUserID(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            _userRepository.AddUser(user);
            return CreatedAtAction(nameof(GetUserByUserID), new { id = user.UserID }, user);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (user == null || id != user.UserID)
            {
                return BadRequest();
            }

            var existingUser = _userRepository.GetUserByUserID(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            _userRepository.UpdateUser(user);
            return NoContent();
        }
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var existingUser = _userRepository.GetUserByUserID(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            _userRepository.DeleteUser(id);
            return NoContent();
        }
        [HttpPost]
        public IActionResult Authenticate([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var existingUser = _userRepository.GetUserByUserID(user.UserID);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (existingUser.Password != user.Password)
            {
                return Unauthorized();
            }

            return Ok(existingUser);
        }
        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            _userRepository.AddUser(user);
            return CreatedAtAction(nameof(GetUserByUserID), new { id = user.UserID }, user);
        }
        [HttpPost]
        public IActionResult ChangePassword([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var existingUser = _userRepository.GetUserByUserID(user.UserID);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Password = user.Password;
            _userRepository.UpdateUser(existingUser);
            return NoContent();
        }
    }
}
