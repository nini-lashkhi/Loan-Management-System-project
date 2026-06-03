using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;

namespace LoanManagement.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<LoanResponseDto>> GetCustomerLoansAsync(int customerId)
    {
        var exists = await _customerRepository.ExistsAsync(customerId);
        if (!exists)
            throw new KeyNotFoundException($"მომხმარებელი ID={customerId} ვერ მოიძებნა.");

        var loans = await _customerRepository.GetLoansByCustomerIdAsync(customerId);

        return loans.Select(l => new LoanResponseDto(
            l.Id, l.CustomerId, l.Amount, l.InterestRate,
            l.TermMonths, l.MonthlyPayment, l.Status.ToString(), l.CreatedAt));
    }
}