// payCityUtilitiesApp.Api/Services/Interfaces/IAuthService.cs
using payCityUtilitiesApp.Api.DTOs;
using System.Threading.Tasks; // For Task

namespace payCityUtilitiesApp.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto request);
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
    }
}