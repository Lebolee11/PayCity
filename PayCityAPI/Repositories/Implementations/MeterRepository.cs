// payCityUtilitiesApp.Api/Repositories/Implementations/MeterRepository.cs
using Microsoft.EntityFrameworkCore;
using PayCityApp.Api.Api.Models;
using payCityUtilitiesApp.Api.Data;
using payCityUtilitiesApp.Api.Repositories.Interfaces;

namespace payCityUtilitiesApp.Api.Repositories.Implementations
{
    public class MeterRepository : IMeterRepository
    {
        private readonly ApplicationDbContext _context;

        public MeterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Meter> GetMeterByIdAsync(Guid id)
        {
            return await _context.Meters.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Meter> GetMeterByMetreIdAsync(string metreId)
        {
            return await _context.Meters.FirstOrDefaultAsync(m => m.MetreId == metreId);
        }

        public async Task<IEnumerable<Meter>> GetMetersByUserIdAsync(Guid userId)
        {
            return await _context.Meters.Where(m => m.UserId == userId).ToListAsync();
        }

        public async Task AddMeterAsync(Meter meter)
        {
            _context.Meters.Add(meter);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMeterAsync(Meter meter)
        {
            _context.Meters.Update(meter);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMeterAsync(Meter meter)
        {
            _context.Meters.Remove(meter);
            await _context.SaveChangesAsync();
        }
    }
}