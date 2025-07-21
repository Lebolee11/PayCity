// payCityUtilitiesApp.Api/Services/Interfaces/IFineService.cs
using payCityUtilitiesApp.Api.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace payCityUtilitiesApp.Api.Services.Interfaces
{
    public interface IFineService
    {
        Task<IEnumerable<FineDto>> GetFinesForUserAsync(Guid userId);
        Task<PayFineResponseDto> PayFineAsync(PayFineRequestDto request, Guid userId);
        // Optionally, add methods to add/update fines for admin purposes if needed
        // Task<FineDto> AddFineAsync(AddFineRequestDto request);
    }
}