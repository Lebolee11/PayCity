// payCityUtilitiesApp.Api/Repositories/Interfaces/ITransactionRepository.cs
using PayCityApp.Api.Models;

namespace payCityUtilitiesApp.Api.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetTransactionByIdAsync(Guid id);
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(Guid userId);
        Task AddTransactionAsync(Transaction transaction);
        // No Update or Delete for transactions usually, as they are historical records
    }
}