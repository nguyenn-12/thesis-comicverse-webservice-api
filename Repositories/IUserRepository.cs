using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using thesis_comicverse_webservice_api.Database;
using thesis_comicverse_webservice_api.DTOs.AuthenticationDTO;
using thesis_comicverse_webservice_api.Models;

namespace thesis_comicverse_webservice_api.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);

        Task<KeyValuePair<string, User>> Login(LoginDTO loginInfor);
        Task<KeyValuePair<string, User>> Register(RegisterDTO registInfor);

    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbcontext;

        private readonly ILogger<ProductRepository> _logger;
        private readonly IConfiguration _configuration;
        public UserRepository(AppDbContext dbcontext, ILogger<ProductRepository> logger, IConfiguration configuration)
        {
            _dbcontext = dbcontext;
            _logger = logger;
            _configuration = configuration;
        }


        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                if (_dbcontext.Users == null) throw new ArgumentNullException(nameof(_dbcontext.Users));

                return await _dbcontext.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve users: {ex.Message}");
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                if (_dbcontext.Users == null) throw new ArgumentNullException(nameof(_dbcontext.Users));

                var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.userId == id);
                if (user == null) throw new ArgumentNullException(nameof(user));

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve users: {ex.Message}");
            }
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            try
            {
                if (_dbcontext.Users == null) throw new ArgumentNullException(nameof(_dbcontext.Users));

                var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.userName == userName);

                return user!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve users: {ex.Message}");
            }
        }


        public async Task<KeyValuePair<string, User>> Login(LoginDTO loginForm)
        {
            try
            {
                if (_dbcontext.Users == null) throw new ArgumentNullException(nameof(_dbcontext.Users));

                var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.userName == loginForm.username && u.hashedPassword == loginForm.password);

                //_logger.LogInformation($"")
                if (user == null) _logger.Log(LogLevel.Information, $"User {loginForm.username} failed to log in at {DateTime.UtcNow.ToLongTimeString()}");

                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("userId", user.userId.ToString()),
                        new Claim("email", user.email.ToString()),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signIn
                    );
                    string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    _logger.Log(LogLevel.Information, $"User {user.userName} logged in at {DateTime.UtcNow.ToLongTimeString()}");

                    return new KeyValuePair<string, User>(tokenString, user);
                }
                else
                {
                    return new KeyValuePair<string, User>("Failed", null!);
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve users: {ex.Message}");
            }

        }


        public async Task<KeyValuePair<string, User>> Register(RegisterDTO registInfor)
        {
            try
            {
                User existUser = GetUserByNameAsync(registInfor.username!).Result;
                _logger.LogInformation("...................");
                if (existUser != null) throw new Exception("User already exists");

                User newUser = new User
                {
                    userName = registInfor.username,
                    email = registInfor.email,
                    hashedPassword = registInfor.password,
                    firstName = "",
                    lastName = "",
                    phoneNumber = "",
                    status = "Active",
                    createdAt = DateTime.UtcNow,
                    role = "User"
                };
                _logger.LogInformation(JsonConvert.SerializeObject(newUser));
                var addedUser = await AddUser(newUser);
                _logger.LogInformation(JsonConvert.SerializeObject(addedUser));

                if (addedUser == null) throw new Exception("Failed to add user");

                return new KeyValuePair<string, User>("Success", addedUser);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve users: {ex.Message}");
            }
        }




        private async Task<User> AddUser(User user)
        {
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user));
                if (_dbcontext.Users == null) throw new ArgumentNullException(nameof(_dbcontext.Users));

                await _dbcontext.Users.AddAsync(user);
                await _dbcontext.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve users: {ex.Message}");
            }
        }

    }
}
