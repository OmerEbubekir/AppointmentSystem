using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Interfaces;
using Data.Context;
using Data.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Bussiness.Services
{
    public class UserService : IUserService
    {
        private readonly AppointmentDbContext _context;

        public UserService(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;

            // Şifre hashini kontrol etmek
            if (VerifyPassword(password, user.PasswordHash))
                return user;

            return null;
        }

        public async Task<bool> RegisterAsync(User user, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return false; // Email zaten kayıtlı

            user.PasswordHash = HashPassword(password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }

        // Şifreyi hash
        private string HashPassword(string password)
        {
            // BCrypt  güvenli hashleme 
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // Hashli sifreyi bakma 
        private bool VerifyPassword(string password, string storedHash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == storedHash;
        }
    }



}
