using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Entities;
using LoanManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Infrastructure.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly AppDbContext _context;

    public LoanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Loan?> GetByIdAsync(int id) =>
        await _context.Loans.Include(l => l.Customer).FirstOrDefaultAsync(l => l.Id == id);

    public async Task<Loan> AddAsync(Loan loan)
    {
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();
        return loan;
    }

    public async Task UpdateAsync(Loan loan)
    {
        _context.Loans.Update(loan);
        await _context.SaveChangesAsync();
    }

    public async Task AddScheduleAsync(IEnumerable<LoanSchedule> schedule)
    {
        _context.LoanSchedules.AddRange(schedule);
        await _context.SaveChangesAsync();
    }
}