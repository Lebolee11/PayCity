// payCityUtilitiesApp.Api/Repositories/Implementations/TransactionRepository.cs
using Microsoft.EntityFrameworkCore;
using PayCityApp.Api.Models;
using payCityUtilitiesApp.Api.Data;
using payCityUtilitiesApp.Api.Repositories.Interfaces;

namespace payCityUtilitiesApp.Api.Repositories.Implementations
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(Guid userId)
        {
            return await _context.Transactions.Where(t => t.UserId == userId).OrderByDescending(t => t.TransactionDate).ToListAsync();
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }
        // No Update or Delete operations as transactions are typically immutable records.
    }
}