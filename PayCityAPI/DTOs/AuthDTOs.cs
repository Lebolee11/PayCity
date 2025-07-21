// payCityUtilitiesApp.Api/DTOs/AuthDTOs.cs
using System; // For Guid

namespace payCityUtilitiesApp.Api.DTOs
{
    public class LoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Message { get; set; }
    }

    public class ForgotPasswordRequestDto
    {
        public string Email { get; set; }
    }

    public class RegisterRequestDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool TermsAccepted { get; set; }
    }

    public class RegisterResponseDto
    {
        public string Message { get; set; }
        public Guid UserId { get; set; }
    }
}