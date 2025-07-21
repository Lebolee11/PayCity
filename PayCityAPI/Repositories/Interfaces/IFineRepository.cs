// payCityUtilitiesApp.Api/Repositories/Interfaces/IFineRepository.cs
using PayCityApp.Api.Models;

namespace payCityUtilitiesApp.Api.Repositories.Interfaces
{
    public interface IFineRepository
    {
        Task<Fine> GetFineByIdAsync(Guid id);
        Task<IEnumerable<Fine>> GetFinesByUserIdAsync(Guid userId);
        Task AddFineAsync(Fine fine);
        Task UpdateFineAsync(Fine fine);
        Task DeleteFineAsync(Fine fine);
    }
}