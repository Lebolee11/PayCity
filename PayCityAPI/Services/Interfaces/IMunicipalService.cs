// payCityUtilitiesApp.Api/Services/Interfaces/IMunicipalService.cs
using payCityUtilitiesApp.Api.DTOs;
using System.Threading.Tasks;
using System; // For Guid

namespace payCityUtilitiesApp.Api.Services.Interfaces
{
    public interface IMunicipalService
    {
        Task<PayMunicipalAccountResponseDto> PayAccountAsync(PayMunicipalAccountRequestDto request, Guid userId);
    }
}