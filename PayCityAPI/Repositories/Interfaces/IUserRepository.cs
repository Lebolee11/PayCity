// payCityUtilitiesApp.Api/Repositories/Interfaces/IUserRepository.cs
using PayCityAppApi.Models;
using System; // For Guid

namespace payCityUtilitiesApp.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(Guid id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}