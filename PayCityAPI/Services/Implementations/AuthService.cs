// payCityUtilitiesApp.Api/Services/Implementations/AuthService.cs
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using payCityUtilitiesApp.Api.DTOs;
using payCityUtilitiesApp.Api.Repositories.Interfaces;
using payCityUtilitiesApp.Api.Services.Interfaces;
using PayCityAppApi.Models;

namespace payCityUtilitiesApp.Api.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || !VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new LoginResponseDto { Message = "Invalid credentials." };
            }

            var token = GenerateJwtToken(user);
            return new LoginResponseDto { Token = token, Message = "Login successful." };
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                Console.WriteLine($"[AuthService] Password reset request for: {request.Email}. User not found, but a generic success message is returned for security.");
                return true;
            }

            // In a real application, you would:
            // 1. Generate a unique, time-limited password reset token.
            // 2. Store this token in the database associated with the user.
            // 3. Send an email to the user with a link containing this token.
            // 4. The user clicks the link, enters new password, which then validates the token.
            Console.WriteLine($"[AuthService] Password reset request for: {request.Email}. (Simulated email sent)");
            return true;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            if (await _userRepository.GetUserByEmailAsync(request.Email) != null)
            {
                return new RegisterResponseDto { Message = "Email already registered." };
            }

            if (!request.TermsAccepted)
            {
                return new RegisterResponseDto { Message = "Terms and conditions must be accepted." };
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User
            {
                Id = Guid.NewGuid(), // Generate a new GUID for the user
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Phone = request.Phone,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                TermsAccepted = request.TermsAccepted,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddUserAsync(newUser);

            return new RegisterResponseDto { Message = "Registration successful.", UserId = newUser.Id };
        }

        // --- Helper Methods for Password Hashing and JWT Token Generation ---

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}