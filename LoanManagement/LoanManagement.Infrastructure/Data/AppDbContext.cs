using LoanManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LoanManagement.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<LoanSchedule> LoanSchedules => Set<LoanSchedule>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Loan>().HasQueryFilter(l => !l.IsDeleted);
        modelBuilder.Entity<Payment>().HasQueryFilter(p => !p.IsDeleted);

        
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.PersonalNumber)
            .IsUnique();

        
        modelBuilder.Entity<Loan>().Property(l => l.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<Loan>().Property(l => l.InterestRate).HasPrecision(5, 2);
        modelBuilder.Entity<Loan>().Property(l => l.MonthlyPayment).HasPrecision(18, 2);
        modelBuilder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<LoanSchedule>().Property(s => s.PMT).HasPrecision(18, 2);
    }
}