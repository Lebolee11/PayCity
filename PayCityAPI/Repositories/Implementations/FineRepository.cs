// payCityUtilitiesApp.Api/Repositories/Implementations/FineRepository.cs
using Microsoft.EntityFrameworkCore;
using PayCityApp.Api.Models;
using payCityUtilitiesApp.Api.Data;
using payCityUtilitiesApp.Api.Repositories.Interfaces;

namespace payCityUtilitiesApp.Api.Repositories.Implementations
{
    public class FineRepository : IFineRepository
    {
        private readonly ApplicationDbContext _context;

        public FineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Fine> GetFineByIdAsync(Guid id)
        {
            return await _context.Fines.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Fine>> GetFinesByUserIdAsync(Guid userId)
        {
            return await _context.Fines.Where(f => f.UserId == userId).ToListAsync();
        }

        public async Task AddFineAsync(Fine fine)
        {
            _context.Fines.Add(fine);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFineAsync(Fine fine)
        {
            _context.Fines.Update(fine);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFineAsync(Fine fine)
        {
            _context.Fines.Remove(fine);
            await _context.SaveChangesAsync();
        }
    }
}