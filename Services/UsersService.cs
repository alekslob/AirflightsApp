
using Airflights.Data;
using Airflights.Models;
using Airflights.Utils;
using Microsoft.EntityFrameworkCore;

namespace Airflights.Services
{
    public interface IUsersService
    {
        Task<UserViewModel?> GetById(int id);
        Task<User?> GetByLogin(string login);
        Task<User> CreateAsync(UserCreateModel data);
        Task<User> UpdateAsync(User data);
    }

    public class UsersService: IUsersService
    {
        private readonly ApplicationDbContext _context;
        public UsersService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UserViewModel?> GetById(int id)
        {
            return await _context.Users.Select(
                user => new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Role = user.Role
                }
            ).FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User?> GetByLogin(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        }
        public async Task<User> CreateAsync(UserCreateModel data)
        {
            // Хешируем пароль
            var passwordHash = PasswordHelper.HashPassword(data.Password);
            var user = new User
            {
                Name = data.Name,
                Login = data.Login,
                Hash = passwordHash,
                Role = data.Role,
                CreatedAt = DateTime.Now
            };
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> UpdateAsync(User data)
        {
            _context.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }

    }
}