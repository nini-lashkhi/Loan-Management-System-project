using LoanManagement.Domain.Enums;

namespace LoanManagement.Domain.Entities;

public class Loan
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermMonths { get; set; }
    public decimal MonthlyPayment { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;

    public Customer Customer { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<LoanSchedule> Schedule { get; set; } = new List<LoanSchedule>();
}