using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<LoanResponseDto>> GetCustomerLoansAsync(int customerId);
}