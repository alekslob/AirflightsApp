
using System.Security.Cryptography;
using System.Text;

namespace Airflights.Utils
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Хеширует пароль с использованием SHA256
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Пароль не может быть пустым");

            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            
            return Convert.ToBase64String(hash);
        }
        /// <summary>
        /// Проверяет соответствие пароля хешу
        /// </summary>
        public static bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
                return false;

            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
        
        /// <summary>
        /// Генерирует случайный пароль (для сброса)
        /// </summary>
        public static string GenerateRandomPassword(int length = 12)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            
            return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
        }
    }
}