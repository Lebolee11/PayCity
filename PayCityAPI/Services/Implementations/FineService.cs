// payCityUtilitiesApp.Api/Services/Implementations/FineService.cs
using PayCityApp.Api.Models;
using payCityUtilitiesApp.Api.DTOs;
using payCityUtilitiesApp.Api.Repositories.Interfaces; // For Fine, Transaction Repositories
using payCityUtilitiesApp.Api.Services.Interfaces;

namespace payCityUtilitiesApp.Api.Services.Implementations
{
    public class FineService : IFineService
    {
        private readonly IFineRepository _fineRepository;
        private readonly ITransactionRepository _transactionRepository;

        public FineService(IFineRepository fineRepository, ITransactionRepository transactionRepository)
        {
            _fineRepository = fineRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<FineDto>> GetFinesForUserAsync(Guid userId)
        {
            var fines = await _fineRepository.GetFinesByUserIdAsync(userId);
            if (fines == null)
            {
                return Enumerable.Empty<FineDto>(); // Return empty if no fines found
            }

            // Map Models to DTOs for the API response
            return fines.Select(f => new FineDto
            {
                Id = f.Id,
                FineNumber = f.FineNumber,
                Description = f.Description,
                Amount = f.Amount,
                IsPaid = f.IsPaid,
                ImageUrl = f.ImageUrl,
                IssueDate = f.IssueDate,
                DueDate = f.DueDate
            });
        }

        public async Task<PayFineResponseDto> PayFineAsync(PayFineRequestDto request, Guid userId)
        {
            var fine = await _fineRepository.GetFineByIdAsync(request.FineId);

            // 1. Validate Fine
            if (fine == null)
            {
                return new PayFineResponseDto { Message = "Fine not found." };
            }
            if (fine.UserId != userId)
            {
                return new PayFineResponseDto { Message = "Fine does not belong to the authenticated user." };
            }
            if (fine.IsPaid)
            {
                return new PayFineResponseDto { Message = "Fine has already been paid." };
            }

            // 2. Simulate Payment Processing
            bool paymentSuccess = SimulatePayment(request.PaymentMethod, fine.Amount);

            if (!paymentSuccess)
            {
                return new PayFineResponseDto { Message = "Payment failed. Please try again." };
            }

            // 3. Update Fine Status
            fine.IsPaid = true;
            await _fineRepository.UpdateFineAsync(fine);

            // 4. Record Transaction
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Amount = fine.Amount,
                TransactionType = "PayFine",
                PaymentMethod = request.PaymentMethod,
                Reference = fine.FineNumber, // Fine number is the reference
                Status = "Success",
                ReceiptUrl = GenerateReceiptUrl(fine.FineNumber, fine.Amount) // Simulate receipt URL
            };
            await _transactionRepository.AddTransactionAsync(transaction);

            return new PayFineResponseDto
            {
                FineId = fine.Id,
                AmountPaid = fine.Amount,
                Message = "Fine paid successfully!",
                ReceiptUrl = transaction.ReceiptUrl
            };
        }

        // --- Helper Methods ---
        private bool SimulatePayment(string paymentMethod, decimal amount)
        {
            return amount > 0;
        }

        private string GenerateReceiptUrl(string fineNumber, decimal amount)
        {
            return $"https://paycity.com/receipts/fine_{fineNumber}_{amount:0.00}.pdf";
        }
    }
}