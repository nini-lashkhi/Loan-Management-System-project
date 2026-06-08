using LoanManagement.Domain.Entities;

namespace LoanManagement.Application.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer?> GetByPersonalNumberAsync(string personalNumber);
    Task<IEnumerable<Loan>> GetLoansByCustomerIdAsync(int customerId);
    Task<Customer> AddAsync(Customer customer);
    Task<bool> ExistsAsync(int id);
}