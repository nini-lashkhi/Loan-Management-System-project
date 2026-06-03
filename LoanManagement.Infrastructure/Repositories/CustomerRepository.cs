using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Entities;
using LoanManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(int id) =>
        await _context.Customers.FindAsync(id);

    public async Task<Customer?> GetByPersonalNumberAsync(string personalNumber) =>
        await _context.Customers.FirstOrDefaultAsync(c => c.PersonalNumber == personalNumber);

    public async Task<IEnumerable<Loan>> GetLoansByCustomerIdAsync(int customerId) =>
        await _context.Loans.Where(l => l.CustomerId == customerId).ToListAsync();

    public async Task<Customer> AddAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _context.Customers.AnyAsync(c => c.Id == id);
}