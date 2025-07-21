// payCityUtilitiesApp.Api/DTOs/UtilitiesDTOs.cs
using System;
using System.ComponentModel.DataAnnotations; // Required for decimal, Guid etc.

namespace payCityUtilitiesApp.Api.DTOs
{
    public class BuyTokenRequestDto
    {
        public string MetreId { get; set; } // ID of the meter
        public decimal Amount { get; set; } // Amount to be prepaid
        public string PaymentMethod { get; set; } // e.g., "credit_card", "eft"
    }

    public class BuyTokenResponseDto
    {
        public string TokenNumber { get; set; } // The generated prepaid token
        public decimal Amount { get; set; } // The amount the token is for
        public string Message { get; set; } // Confirmation message
        public string ReceiptUrl { get; set; } // Optional: URL to a generated receipt
    }

        public class RegisterMeterRequestDto // <--- NEW DTO
    {
        [Required]
        public string MetreId { get; set; } // The actual physical meter ID/number
        [Required]
        public string Type { get; set; } // e.g., "Electricity", "Water"
        [Required]
        public string Location { get; set; } // Physical address or description
    }

    public class RegisterMeterResponseDto // <--- NEW DTO
    {
        public string Message { get; set; }
        public Guid MeterId { get; set; }
        public string MetreId { get; set; }
    }
}