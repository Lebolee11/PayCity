// payCityUtilitiesApp.Api/Controllers/MunicipalController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using payCityUtilitiesApp.Api.DTOs;
using payCityUtilitiesApp.Api.Services.Interfaces;
using System.Security.Claims; // For ClaimTypes
using System; // For Guid
using System.Threading.Tasks;

namespace payCityUtilitiesApp.Api.Controllers
{
    [ApiController]
    [Route("api/municipal")]
    [Authorize] // All actions in this controller require authentication
    public class MunicipalController : ControllerBase
    {
        private readonly IMunicipalService _municipalService;

        public MunicipalController(IMunicipalService municipalService)
        {
            _municipalService = municipalService;
        }

        /// <summary>
        /// Allows an authenticated user to pay a municipal account.
        /// </summary>
        /// <param name="request">Details of the municipal account payment request.</param>
        /// <returns>A confirmation of the payment or an error message.</returns>
        [HttpPost("pay-account")]
        [ProducesResponseType(typeof(PayMunicipalAccountResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> PayAccount([FromBody] PayMunicipalAccountRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the User ID from the JWT token (from authenticated user claims)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user ID in token." });
            }

            var response = await _municipalService.PayAccountAsync(request, userId);

            if (response.AmountPaid == 0 && !string.IsNullOrEmpty(response.Message)) // Indicates an error or failure from service
            {
                return BadRequest(new { message = response.Message });
            }

            return Ok(response);
        }

        // You could add other municipal-related endpoints here, e.g.,
        // [HttpGet("my-accounts")] to list municipal accounts linked to the user
        // [HttpGet("account-details/{accountNumber}")] to get balance/details of an account
    }
}