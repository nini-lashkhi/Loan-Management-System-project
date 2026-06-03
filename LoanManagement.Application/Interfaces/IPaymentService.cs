using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Interfaces;

public interface IPaymentService
{
    Task<PaymentResponseDto> MakePaymentAsync(CreatePaymentDto dto);
}