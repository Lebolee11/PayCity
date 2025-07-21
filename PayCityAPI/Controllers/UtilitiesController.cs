// payCityUtilitiesApp.Api/Controllers/UtilitiesController.cs
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
    [Route("api/utilities")]
    [Authorize] // All actions in this controller require authentication
    public class UtilitiesController : ControllerBase
    {
        private readonly IUtilityService _utilityService;

        public UtilitiesController(IUtilityService utilityService)
        {
            _utilityService = utilityService;
        }

        /// <summary>
        /// Allows an authenticated user to purchase a prepaid utility token for a specified meter.
        /// </summary>
        /// <param name="request">Details of the token purchase request.</param>
        /// <returns>A confirmation of the token purchase or an error message.</returns>
        [HttpPost("buy-token")]
        [ProducesResponseType(typeof(BuyTokenResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> BuyToken([FromBody] BuyTokenRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the User ID from the JWT token (from authenticated user claims)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                // This scenario should ideally not happen with a valid token, but good for robustness
                return Unauthorized(new { message = "Invalid user ID in token." });
            }

            var response = await _utilityService.BuyTokenAsync(request, userId);

            if (response.TokenNumber == null) // Indicates an error or failure from service
            {
                return BadRequest(new { message = response.Message });
            }

            return Ok(response);
        }

        /// <summary>
        /// Allows an authenticated user to register a new utility meter to their account.
        /// </summary>
        /// <param name="request">Details of the meter to register.</param>
        /// <returns>Confirmation of registration or an error if the meter already exists/is linked.</returns>
        [HttpPost("register-meter")] // <--- NEW ENDPOINT
        [ProducesResponseType(typeof(RegisterMeterResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> RegisterMeter([FromBody] RegisterMeterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user ID in token." });
            }

            var response = await _utilityService.RegisterMeterAsync(request, userId);

            // Check for specific error messages from the service to return appropriate status
            if (!string.IsNullOrEmpty(response.Message) && response.Message.Contains("already registered to another user", StringComparison.OrdinalIgnoreCase))
            {
                return Conflict(new { message = response.Message }); // 409 Conflict
            }
            if (!string.IsNullOrEmpty(response.Message) && response.Message.Contains("already registered to your account", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(response); // Still a success, just re-confirming existing link
            }
            if (response.MeterId == Guid.Empty) // Generic service error
            {
                return BadRequest(new { message = response.Message });
            }


            return CreatedAtAction(nameof(RegisterMeter), new { id = response.MeterId }, response); // 201 Created
        }

        // You could add other utility-related endpoints here, e.g.,
        // [HttpGet("my-meters")] to list meters associated with the user
        // [HttpPost("register-meter")] to allow users to register new meters
    }
}