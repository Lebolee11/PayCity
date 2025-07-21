// payCityUtilitiesApp.Api/Services/Implementations/MunicipalService.cs
using PayCityApp.Api.Models;
using payCityUtilitiesApp.Api.DTOs;
using payCityUtilitiesApp.Api.Repositories.Interfaces; // For Transaction Repository
using payCityUtilitiesApp.Api.Services.Interfaces;

namespace payCityUtilitiesApp.Api.Services.Implementations
{
    public class MunicipalService : IMunicipalService
    {
        private readonly ITransactionRepository _transactionRepository;

        public MunicipalService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<PayMunicipalAccountResponseDto> PayAccountAsync(PayMunicipalAccountRequestDto request, Guid userId)
        {
            // 1. Validate Account Number (In a real app, you'd likely call an external municipal API)
            if (string.IsNullOrWhiteSpace(request.AccountNumber))
            {
                return new PayMunicipalAccountResponseDto { Message = "Account number cannot be empty." };
            }
            // Simulate account validation: assume valid if it's not "INVALID123"
            if (request.AccountNumber.Equals("INVALID123", StringComparison.OrdinalIgnoreCase))
            {
                 return new PayMunicipalAccountResponseDto { Message = "Invalid municipal account number." };
            }

            // 2. Simulate Payment Processing
            bool paymentSuccess = SimulatePayment(request.PaymentMethod, request.Amount);

            if (!paymentSuccess)
            {
                return new PayMunicipalAccountResponseDto { Message = "Payment failed. Please try again." };
            }

            // 3. Record Transaction
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Amount = request.Amount,
                TransactionType = "PayMunicipalAccount",
                PaymentMethod = request.PaymentMethod,
                Reference = request.AccountNumber, // Account number is the reference
                Status = "Success",
                ReceiptUrl = GenerateReceiptUrl(request.AccountNumber, request.Amount) // Simulate receipt URL
            };
            await _transactionRepository.AddTransactionAsync(transaction);

            return new PayMunicipalAccountResponseDto
            {
                AccountNumber = request.AccountNumber,
                AmountPaid = request.Amount,
                Message = "Municipal account paid successfully!",
                ReceiptUrl = transaction.ReceiptUrl
            };
        }

        // --- Helper Methods ---
        private bool SimulatePayment(string paymentMethod, decimal amount)
        {
            return amount > 0;
        }

        private string GenerateReceiptUrl(string accountNumber, decimal amount)
        {
            return $"https://paycity.com/receipts/municipal_{accountNumber}_{amount:0.00}.pdf";
        }
    }
}