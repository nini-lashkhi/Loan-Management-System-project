namespace LoanManagement.Application.DTOs;

public record CreateLoanApplicationDto(
    int CustomerId,
    decimal Amount,
    decimal InterestRate,
    int TermMonths
);

public record LoanResponseDto(
    int Id,
    int CustomerId,
    decimal Amount,
    decimal InterestRate,
    int TermMonths,
    decimal MonthlyPayment,
    string Status,
    DateTime CreatedAt
);