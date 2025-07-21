// payCityUtilitiesApp.Api/DTOs/MunicipalDTOs.cs
using System; // Required for decimal

namespace payCityUtilitiesApp.Api.DTOs
{
    public class PayMunicipalAccountRequestDto
    {
        public string AccountNumber { get; set; } // Municipal account number
        public decimal Amount { get; set; } // Bill amount to be paid
        public string PaymentMethod { get; set; } // e.g., "credit_card", "eft"
    }

    public class PayMunicipalAccountResponseDto
    {
        public string Message { get; set; } // Confirmation message
        public string AccountNumber { get; set; } // The municipal account number paid
        public decimal AmountPaid { get; set; } // The actual amount paid
        public string ReceiptUrl { get; set; } // Optional: URL to a generated receipt
    }
}