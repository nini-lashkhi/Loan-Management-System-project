using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;

namespace LoanManagement.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILoanRepository _loanRepository;

    public PaymentService(IPaymentRepository paymentRepository, ILoanRepository loanRepository)
    {
        _paymentRepository = paymentRepository;
        _loanRepository = loanRepository;
    }

    public async Task<PaymentResponseDto> MakePaymentAsync(CreatePaymentDto dto)
    {
        var loan = await _loanRepository.GetByIdAsync(dto.LoanId)
            ?? throw new KeyNotFoundException($"სესხი ID={dto.LoanId} ვერ მოიძებნა.");

        if (loan.Status == LoanStatus.Closed)
            throw new InvalidOperationException("დახურულ სესხზე გადახდა შეუძლებელია.");

        if (loan.Status is LoanStatus.Rejected or LoanStatus.Pending)
            throw new InvalidOperationException("ამ სტატუსის სესხზე გადახდა შეუძლებელია.");

        var payment = new Payment
        {
            LoanId = dto.LoanId,
            Amount = dto.Amount,
            PaymentDate = DateTime.UtcNow
        };

        var created = await _paymentRepository.AddAsync(payment);

        var allPayments = await _paymentRepository.GetByLoanIdAsync(dto.LoanId);
        var totalPaid = allPayments.Sum(p => p.Amount);
        var totalDue = loan.MonthlyPayment * loan.TermMonths;

        if (totalPaid >= totalDue)
        {
            loan.Status = LoanStatus.Closed;
            await _loanRepository.UpdateAsync(loan);
        }

        return new PaymentResponseDto(created.Id, created.LoanId, created.Amount, created.PaymentDate);
    }
}