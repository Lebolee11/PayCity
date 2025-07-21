// payCityUtilitiesApp.Api/Services/Implementations/UtilityService.cs
using PayCityApp.Api.Api.Models;
using PayCityApp.Api.Models;
using payCityUtilitiesApp.Api.DTOs;
using payCityUtilitiesApp.Api.Repositories.Interfaces; // For Meter, Transaction Repositories
using payCityUtilitiesApp.Api.Services.Interfaces;

namespace payCityUtilitiesApp.Api.Services.Implementations
{
    public class UtilityService : IUtilityService
    {
        private readonly IMeterRepository _meterRepository;
        private readonly ITransactionRepository _transactionRepository;

        public UtilityService(IMeterRepository meterRepository, ITransactionRepository transactionRepository)
        {
            _meterRepository = meterRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<BuyTokenResponseDto> BuyTokenAsync(BuyTokenRequestDto request, Guid userId)
        {
            // 1. Validate Meter
            var meter = await _meterRepository.GetMeterByMetreIdAsync(request.MetreId);
            if (meter == null)
            {
                // Optionally, if the meter doesn't exist, create it or return an error
                // For now, let's assume meters should exist beforehand or throw an error.
                return new BuyTokenResponseDto { Message = "Meter not found." };
            }

            // Ensure the meter belongs to the authenticated user (security check)
            if (meter.UserId != userId)
            {
                return new BuyTokenResponseDto { Message = "Meter does not belong to the authenticated user." };
            }

            // 2. Simulate Payment Processing (In a real app, integrate with a payment gateway here)
            bool paymentSuccess = SimulatePayment(request.PaymentMethod, request.Amount);

            if (!paymentSuccess)
            {
                return new BuyTokenResponseDto { Message = "Payment failed. Please try again." };
            }

            // 3. Generate Token
            string tokenNumber = GenerateUniqueToken(); // Implement your token generation logic

            // 4. Record Transaction
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Amount = request.Amount,
                TransactionType = "BuyToken",
                PaymentMethod = request.PaymentMethod,
                Reference = tokenNumber, // The token number is the reference
                Status = "Success",
                ReceiptUrl = GenerateReceiptUrl(tokenNumber, request.Amount, request.MetreId) // Simulate receipt URL
            };
            await _transactionRepository.AddTransactionAsync(transaction);

            return new BuyTokenResponseDto
            {
                TokenNumber = tokenNumber,
                Amount = request.Amount,
                Message = "Prepaid token purchased successfully!",
                ReceiptUrl = transaction.ReceiptUrl
            };
        }

        public async Task<RegisterMeterResponseDto> RegisterMeterAsync(RegisterMeterRequestDto request, Guid userId) // <--- NEW METHOD IMPLEMENTATION
        {
            // Check if a meter with this MetreId already exists globally
            var existingMeter = await _meterRepository.GetMeterByMetreIdAsync(request.MetreId);
            if (existingMeter != null)
            {
                // If it exists, check if it's already linked to this user
                if (existingMeter.UserId == userId)
                {
                    return new RegisterMeterResponseDto
                    {
                        Message = "This meter is already registered to your account.",
                        MeterId = existingMeter.Id,
                        MetreId = existingMeter.MetreId
                    };
                }
                else
                {
                    return new RegisterMeterResponseDto
                    {
                        Message = "This meter ID is already registered to another user.",
                        MeterId = Guid.Empty, // Indicate failure
                        MetreId = request.MetreId
                    };
                }
            }

            // If not found, create a new meter
            var newMeter = new Meter
            {
                Id = Guid.NewGuid(),
                UserId = userId, // Link to the authenticated user
                MetreId = request.MetreId,
                Type = request.Type,
                Location = request.Location,
                InstallationDate = DateTime.UtcNow // Set current date/time
            };

            await _meterRepository.AddMeterAsync(newMeter);

            return new RegisterMeterResponseDto
            {
                Message = "Meter registered successfully!",
                MeterId = newMeter.Id,
                MetreId = newMeter.MetreId
            };
        }

        // --- Helper Methods ---
        private bool SimulatePayment(string paymentMethod, decimal amount)
        {
            // In a real application, this would call out to a payment gateway API (e.g., PayFast, Stripe, PayGate)
            // For demonstration, let's always succeed unless amount is zero or negative.
            return amount > 0;
        }

        private string GenerateUniqueToken()
        {
            // This is a very basic token generation. In a real system, tokens are often
            // generated by the utility provider's system and retrieved via an API call.
            // Or they follow specific algorithms (e.g., checksums, certain formats).
            return $"PPC-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}-{DateTime.Now.Ticks % 10000}";
        }

        private string GenerateReceiptUrl(string token, decimal amount, string meterId)
        {
            // Simulate a receipt URL. In reality, this would point to a static file,
            // a dynamically generated PDF, or a receipt view in your frontend.
            return $"https://paycity.com/receipts/{token.Replace("-", "")}_{amount:0.00}_{meterId}.pdf";
        }
    }
}