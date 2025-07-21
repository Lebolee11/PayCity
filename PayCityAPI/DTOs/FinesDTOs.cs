// payCityUtilitiesApp.Api/DTOs/FinesDTOs.cs
using System; // Required for decimal, Guid

namespace payCityUtilitiesApp.Api.DTOs
{
    public class FineDto // Used to display fine details (e.g., in View Fines)
    {
        public Guid Id { get; set; }
        public string FineNumber { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public string ImageUrl { get; set; } // URL to fine image
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class PayFineRequestDto
    {
        public Guid FineId { get; set; } // ID of the fine to be paid
        public string PaymentMethod { get; set; } // e.g., "credit_card", "eft"
    }

    public class PayFineResponseDto
    {
        public string Message { get; set; } // Confirmation message
        public Guid FineId { get; set; } // ID of the fine that was paid
        public decimal AmountPaid { get; set; } // The amount paid for the fine
        public string ReceiptUrl { get; set; } // Optional: URL to a generated receipt
    }
}