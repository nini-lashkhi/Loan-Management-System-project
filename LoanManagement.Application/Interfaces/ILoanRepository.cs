using LoanManagement.Domain.Entities;

namespace LoanManagement.Application.Interfaces;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(int id);
    Task<Loan> AddAsync(Loan loan);
    Task UpdateAsync(Loan loan);
    Task AddScheduleAsync(IEnumerable<LoanSchedule> schedule);
}