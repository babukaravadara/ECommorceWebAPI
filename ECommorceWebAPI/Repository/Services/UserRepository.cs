using ECommorceWebAPI.Data;
using ECommorceWebAPI.Models;
using ECommorceWebAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace ECommorceWebAPI.Repository.Services
{
    public class UserRepository:IUser
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User?>LoginAsync( string email,string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email == email &&
                    x.Password == password
                );
        }
        public async Task SaveRefreshTokenAsync(int userId,string refreshToken)
        {
            var token = new RefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                ExpiryDate = DateTime.Now.AddDays(7),
                CreatedDate =
                    DateTime.Now,
                IsRevoked = false
            };
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
        {

            return await _context.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x =>
                    x.Token == refreshToken &&
                    x.IsRevoked == false
                );
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<object> AddAsync(User user)
        {
            var existingUser = await _context.Users .FirstOrDefaultAsync(x => x.Email == user.Email);
            if (existingUser != null)
            {
                return new
                {
                    success = false,
                    message = "Email Id already exists"
                };
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new
            {
                success = false,
                message = "Registration successful"
            };
        }
        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<object> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return new
            {
                success = true,
                message = "User Deleted Successfully"
            };
        }
    }
}
