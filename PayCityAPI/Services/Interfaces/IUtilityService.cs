// payCityUtilitiesApp.Api/Services/Interfaces/IUtilityService.cs
using payCityUtilitiesApp.Api.DTOs;
using System.Threading.Tasks;
using System; // For Guid

namespace payCityUtilitiesApp.Api.Services.Interfaces
{
    public interface IUtilityService
    {
        Task<BuyTokenResponseDto> BuyTokenAsync(BuyTokenRequestDto request, Guid userId);
        Task<RegisterMeterResponseDto> RegisterMeterAsync(RegisterMeterRequestDto request, Guid userId);
    }
}