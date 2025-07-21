// payCityUtilitiesApp.Api/Repositories/Interfaces/IMeterRepository.cs
using PayCityApp.Api.Api.Models;

namespace payCityUtilitiesApp.Api.Repositories.Interfaces
{
    public interface IMeterRepository
    {
        Task<Meter> GetMeterByIdAsync(Guid id);
        Task<Meter> GetMeterByMetreIdAsync(string metreId); // To find meter by its unique ID
        Task<IEnumerable<Meter>> GetMetersByUserIdAsync(Guid userId);
        Task AddMeterAsync(Meter meter);
        Task UpdateMeterAsync(Meter meter);
        Task DeleteMeterAsync(Meter meter);
    }
}