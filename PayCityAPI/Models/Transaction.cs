// payCityUtilitiesApp.Api/Models/Transaction.cs
using System;
using PayCityAppApi.Models;

namespace PayCityApp.Api.Models
{
    public class Transaction
    {
        public Guid Id { get; set; } // Primary Key for the Transaction
        public decimal Amount { get; set; } // Amount of the transaction

        public string TransactionType { get; set; } // e.g., "BuyToken", "PayMunicipalAccount", "PayFine"
        public string PaymentMethod { get; set; } // e.g., "credit_card", "eft", "wallet"
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow; // When the transaction occurred

        public string Reference { get; set; } // A unique reference for the specific transaction (e.g., token number, account number, fine ID)
        public string Status { get; set; } // e.g., "Success", "Failed", "Pending", "Refunded"
        public string ReceiptUrl { get; set; } // Optional: URL to a generated receipt for the transaction

        // Foreign Key to the User who initiated the transaction
        public Guid UserId { get; set; }
        public User User { get; set; } // Navigation property to the User
    }
}