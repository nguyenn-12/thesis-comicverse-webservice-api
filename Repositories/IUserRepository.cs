using Microsoft.EntityFrameworkCore;
using System;
using thesis_comicverse_webservice_api.Database;
using thesis_comicverse_webservice_api.Models;

namespace thesis_comicverse_webservice_api.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User user);
        void DeleteUser(int id);
        List<User> GetAllUsers();
        User GetUserByUserID(int id);
        void UpdateUser(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        private readonly ILogger<UserRepository> _logger;
        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<User> GetAllUsers()
        {
            try
            {
                //Condition
                if (_context.Users == null) throw new ArgumentNullException(nameof(_context.Users));

                _logger.LogInformation("aaaaaaaaaaaaaaa {DT}", DateTime.UtcNow.ToLongTimeString());
                // Return
                return _context.Users.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve products: {ex.Message}");
            }
        }

        public Product GetUserByUserID(int id)
        {
            try
            {
                //Condition
                if (_context.Users == null) throw new ArgumentNullException(nameof(_context.Users));

                // Do something
                var user = _context.Users!.FirstOrDefault(p => p.UserID == id);

                // Verify
                if (user == null) throw new ArgumentNullException(nameof(user));

                // Return
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve product: {ex.Message}");
            }
        }

        public void AddUser(Product product)
        {
            try
            {
                //Condition
                if (user == null) throw new ArgumentNullException(nameof(user));


                // Do something
                _context.Users!.Add(user);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't add user: {ex.Message}");
            }
        }

        public void UpdateUser(User user)
        {
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user));

                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't update user: {ex.Message}");
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                if (_context.Users == null) throw new ArgumentNullException(nameof(_context.Users));

                var user = _context.User.Find(id);

                if (user == null) throw new ArgumentNullException(nameof(user));

                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't delete user: {ex.Message}");
            }
        }

        void IUserRepository.AddUser(User user)
        {
            throw new NotImplementedException();
        }

        void IUserRepository.DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        List<User> IUserRepository.GetAllUsers()
        {
            throw new NotImplementedException();
        }

        User IUserRepository.GetUserByUserID(int id)
        {
            throw new NotImplementedException();
        }

        void IUserRepository.UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
