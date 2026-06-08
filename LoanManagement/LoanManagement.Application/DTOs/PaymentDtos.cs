namespace LoanManagement.Application.DTOs;

public record CreatePaymentDto(
    int LoanId,
    decimal Amount
);

public record PaymentResponseDto(
    int Id,
    int LoanId,
    decimal Amount,
    DateTime PaymentDate
);