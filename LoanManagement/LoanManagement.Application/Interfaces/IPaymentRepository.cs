using LoanManagement.Domain.Entities;

namespace LoanManagement.Application.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> AddAsync(Payment payment);
    Task<IEnumerable<Payment>> GetByLoanIdAsync(int loanId);
}