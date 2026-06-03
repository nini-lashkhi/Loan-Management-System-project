namespace LoanManagement.Domain.Entities;

public class LoanSchedule
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public decimal PMT { get; set; }
    public DateTime Date { get; set; }
    public bool IsPaid { get; set; } = false;

    public Loan Loan { get; set; } = null!;
}