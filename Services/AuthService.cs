using Airflights.Data;
using Airflights.Models;
using Airflights.Utils;
using Microsoft.EntityFrameworkCore;

namespace Airflights.Services
{
    public interface IAuthService
    {
        Task<UserViewModel?> AuthenticateAsync(string login, string password);
    }

    public class AuthService: IAuthService
    {
        private readonly ApplicationDbContext _context;
        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UserViewModel?> AuthenticateAsync(string login, string password)
        { 
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.IsActive);
            if (user == null) return null;
            if (!PasswordHelper.VerifyPassword(password, user.Hash)) return null;
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new UserViewModel{
                Id = user.Id,
                Name = user.Name,
                Role = user.Role
            };
        } 
    }
} 