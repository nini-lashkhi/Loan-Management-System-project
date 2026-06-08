namespace LoanManagement.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;

    public Loan Loan { get; set; } = null!;
}