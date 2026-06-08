using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Entities;
using LoanManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<IEnumerable<Payment>> GetByLoanIdAsync(int loanId) =>
        await _context.Payments.Where(p => p.LoanId == loanId).ToListAsync();
}
