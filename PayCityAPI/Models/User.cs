// payCityUtilitiesApp.Api/Models/User.cs
using System; // Ensure this is present for Guid and DateTime

namespace PayCityAppApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool TermsAccepted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}