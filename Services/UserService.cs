using Microsoft.EntityFrameworkCore;
using JobBoard.Data;
using JobBoard.Models;

namespace JobBoard.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetProfileAsync(int userId)
        {
            return await _context.Users.FindAsync(userId)
                ?? throw new Exception("User not found");
        }

        public async Task<User> UpdateProfileAsync(int userId, string profileDetails)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            await _context.SaveChangesAsync();

            return user;
        }
    }
}