namespace LoanManagement.Application.DTOs;

public record CreateCustomerDto(
    string FirstName,
    string LastName,
    string PersonalNumber,
    DateTime BirthDate,
    int CreditScore
);

public record CustomerResponseDto(
    int Id,
    string FirstName,
    string LastName,
    string PersonalNumber,
    DateTime BirthDate,
    int CreditScore
);