// payCityUtilitiesApp.Api/Models/Fine.cs
using System;
using PayCityAppApi.Models;

namespace PayCityApp.Api.Models
{
    public class Fine
    {
        public Guid Id { get; set; } // Primary Key for the Fine
        public string FineNumber { get; set; } // The official fine number (e.g., traffic fine number)
        public string Description { get; set; } // Description of the fine (e.g., "Speeding, 80 in a 60 zone")
        public decimal Amount { get; set; } // The monetary amount of the fine
        public bool IsPaid { get; set; } = false; // Status of the fine payment
        public string ImageUrl { get; set; } // Optional: URL to an image related to the fine (e.g., speed camera photo)
        public DateTime IssueDate { get; set; } = DateTime.UtcNow; // When the fine was issued
        public DateTime DueDate { get; set; } // When the fine is due

        // Foreign Key to the User who incurred the fine
        public Guid UserId { get; set; }
        public User User { get; set; } // Navigation property to the User
    }
}