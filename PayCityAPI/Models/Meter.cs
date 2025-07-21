// payCityUtilitiesApp.Api/Models/Meter.cs
using System;
using PayCityAppApi.Models;

namespace PayCityApp.Api.Api.Models
{
    public class Meter
    {
        public Guid Id { get; set; } // Primary Key for the Meter record in the system
        public string MetreId { get; set; } // The actual physical meter ID/number (e.g., "12345678901")
        public string Type { get; set; } // e.g., "Electricity", "Water"
        public string Location { get; set; } // Physical address or description of meter location
        public DateTime InstallationDate { get; set; } = DateTime.UtcNow;

        // Foreign Key to the User who owns or is responsible for this meter
        public Guid UserId { get; set; }
        public User User { get; set; } // Navigation property to the User
    }
}