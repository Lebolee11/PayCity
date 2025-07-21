// payCityUtilitiesApp.Api/Controllers/TestController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // For ClaimTypes

namespace payCityUtilitiesApp.Api.Controllers
{
    [ApiController]
    [Route("api/test")]
    [Authorize] // This entire controller requires authentication
    public class TestController : ControllerBase
    {
        [HttpGet("authenticated-data")]
        public IActionResult GetAuthenticatedData()
        {
            // Access user claims after successful authentication
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new
            {
                message = "You have accessed authenticated data!",
                userId = userId,
                userEmail = userEmail,
                userName = userName,
                currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") // Johannesburg time is UTC+2
            });
        }

        // You could add actions with specific roles here later: [Authorize(Roles = "Admin")]
    }
}