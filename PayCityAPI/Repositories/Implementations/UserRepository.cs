// payCityUtilitiesApp.Api/Repositories/Implementations/UserRepository.cs
using Microsoft.EntityFrameworkCore;
using payCityUtilitiesApp.Api.Data;
using PayCityAppApi.Models;
using payCityUtilitiesApp.Api.Repositories.Interfaces;
using System; // For Guid

namespace payCityUtilitiesApp.Api.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}