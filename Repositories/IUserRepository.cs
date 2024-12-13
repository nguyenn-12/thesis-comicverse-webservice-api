using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        Task<User> DeleteUserAsync(int id);
        Task<User> UpdateUserAsync(User user);
        Task<KeyValuePair<string, User>> Login(LoginDTO loginInfor);
        Task<KeyValuePair<string, User>> Register(RegisterDTO registInfor);
        string GetMyName();
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbcontext;

        private readonly ILogger<ComicRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(AppDbContext dbcontext, ILogger<ComicRepository> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbcontext;
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<User> DeleteUserAsync(int id)
        {
            try
            {
                if (_dbcontext.Users == null) throw new ArgumentNullException(nameof(_dbcontext.Users));

                var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.userId == id);
                if (user == null) throw new ArgumentNullException(nameof(user));

                _dbcontext.Users.Remove(user);
                await _dbcontext.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve users: {ex.Message}");
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user));
                if (_dbcontext.Users == null) throw new ArgumentNullException(nameof(_dbcontext.Users));

                _dbcontext.Users.Update(user);
                await _dbcontext.SaveChangesAsync();

                return user;
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

                var user = await _dbcontext.Users.FirstOrDefaultAsync(u => (u.userName == loginForm.username && u.hashedPassword == loginForm.password) ||
                                                                           (u.email == loginForm.username && u.hashedPassword == loginForm.password));

                //_logger.LogInformation($"")
                if (user == null)
                {
                    _logger.LogError($"User {loginForm.username} failed to log in at {DateTime.UtcNow.ToLongTimeString()}");
                    throw new ArgumentNullException(nameof(user));
                }

                if (user != null)
                {
                    string jwtToken = CreateToken(user);

                    _logger.Log(LogLevel.Information, $"User {user.userName} logged in at {DateTime.UtcNow.ToLongTimeString()}");

                    return new KeyValuePair<string, User>(jwtToken, user);
                }
                else
                {
                    return new KeyValuePair<string, User>("Failed", null!);
                }

            }
            catch
            {
                throw;
            }

        }


        public async Task<KeyValuePair<string, User>> Register(RegisterDTO registInfor)
        {
            try
            {
                User existUser = GetUserByNameAsync(registInfor.username!).Result;
                _logger.LogInformation("...................");
                if (existUser != null) throw new ApplicationException("User already exists");

                User newUser = new User
                {
                    userName = registInfor.username,
                    email = registInfor.email,
                    hashedPassword = registInfor.password,
                    firstName = registInfor.firstName ?? "",
                    lastName = registInfor.lastName ?? "",
                    phoneNumber = registInfor.phoneNumber ?? "",
                    status = "Active",
                    createdAt = DateTime.UtcNow,
                    role = registInfor.role ?? "User"
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

        public string GetMyName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return result;
        }

        private string CreateToken(User user)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("userId", user.userId.ToString()),
                    new Claim("email", user.email!.ToString() ?? "userName", user.userName!.ToString()),
                    new Claim(ClaimTypes.Role, user.role!)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(12),
                    signingCredentials: signIn
                );

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return tokenString;
            }
            catch
            {
                throw;
            }
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            try
            {
                using (var hmac = new HMACSHA512())
                {
                    passwordSalt = hmac.Key;
                    passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                }
            }
            catch
            {
                throw;
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            try
            {
                using (var hmac = new HMACSHA512(passwordSalt))
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return computedHash.SequenceEqual(passwordHash);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
