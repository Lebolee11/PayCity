// payCityUtilitiesApp.Api/Controllers/FinesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using payCityUtilitiesApp.Api.DTOs;
using payCityUtilitiesApp.Api.Services.Interfaces;
using System.Security.Claims; // For ClaimTypes
using System; // For Guid
using System.Collections.Generic;
using System.Threading.Tasks;

namespace payCityUtilitiesApp.Api.Controllers
{
    [ApiController]
    [Route("api/fines")]
    [Authorize] // All actions in this controller require authentication
    public class FinesController : ControllerBase
    {
        private readonly IFineService _fineService;

        public FinesController(IFineService fineService)
        {
            _fineService = fineService;
        }

        /// <summary>
        /// Retrieves a list of fines associated with the authenticated user.
        /// </summary>
        /// <returns>A list of FineDto objects or a "not found" message.</returns>
        [HttpGet("view")]
        [ProducesResponseType(typeof(IEnumerable<FineDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ViewFines()
        {
            // Get the User ID from the JWT token (from authenticated user claims)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user ID in token." });
            }

            var fines = await _fineService.GetFinesForUserAsync(userId);
            if (fines == null || !fines.Any()) // Using .Any() is safer than !fines.ToList().Any()
            {
                return NotFound(new { message = "No fines found for this user." });
            }
            return Ok(fines);
        }

        /// <summary>
        /// Allows an authenticated user to pay a specific fine.
        /// </summary>
        /// <param name="request">Details of the fine payment request.</param>
        /// <returns>A confirmation of the payment or an error message.</returns>
        [HttpPost("pay")]
        [ProducesResponseType(typeof(PayFineResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PayFine([FromBody] PayFineRequestDto request)
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

            var response = await _fineService.PayFineAsync(request, userId);

            // Service returns null if fine not found/user mismatch
            if (response == null)
            {
                return NotFound(new { message = "Fine not found or could not be processed for this user." });
            }
            // Check for specific error messages from the service
            if (!string.IsNullOrEmpty(response.Message) && response.Message.Contains("failed", StringComparison.OrdinalIgnoreCase))
            {
                 return BadRequest(response); // Return specific error from service
            }
            if (!string.IsNullOrEmpty(response.Message) && response.Message.Contains("already been paid", StringComparison.OrdinalIgnoreCase))
            {
                 return BadRequest(response);
            }
            if (!string.IsNullOrEmpty(response.Message) && response.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                return NotFound(response);
            }


            return Ok(response);
        }

        // Optionally, add endpoints for admins to add/update/delete fines
        // [HttpPost("add")]
        // [Authorize(Roles = "Admin")] // Example of role-based authorization
        // public async Task<IActionResult> AddFine([FromBody] AddFineRequestDto request) { ... }
    }
}