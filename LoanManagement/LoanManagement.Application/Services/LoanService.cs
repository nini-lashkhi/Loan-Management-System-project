using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;

namespace LoanManagement.Application.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly ICustomerRepository _customerRepository;

    public LoanService(ILoanRepository loanRepository, ICustomerRepository customerRepository)
    {
        _loanRepository = loanRepository;
        _customerRepository = customerRepository;
    }

    public async Task<LoanResponseDto> CreateApplicationAsync(CreateLoanApplicationDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId)
            ?? throw new KeyNotFoundException($"მომხმარებელი ID={dto.CustomerId} ვერ მოიძებნა.");

        var age = CalculateAge(customer.BirthDate);
        if (age < 18)
            throw new InvalidOperationException("მომხმარებლის ასაკი უნდა იყოს 18 წელი ან მეტი.");

        var status = customer.CreditScore < 300
            ? LoanStatus.Rejected
            : LoanStatus.Approved;

        var monthlyPayment = CalculateMonthlyPayment(dto.Amount, dto.InterestRate, dto.TermMonths);

        var loan = new Loan
        {
            CustomerId = dto.CustomerId,
            Amount = dto.Amount,
            InterestRate = dto.InterestRate,
            TermMonths = dto.TermMonths,
            MonthlyPayment = monthlyPayment,
            Status = status,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _loanRepository.AddAsync(loan);

        if (status == LoanStatus.Approved)
        {
            var schedule = GenerateSchedule(created.Id, monthlyPayment, dto.TermMonths);
            await _loanRepository.AddScheduleAsync(schedule);
        }

        return MapToResponse(created);
    }

    public async Task<LoanResponseDto> GetByIdAsync(int id)
    {
        var loan = await _loanRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"სესხი ID={id} ვერ მოიძებნა.");
        return MapToResponse(loan);
    }

    private static decimal CalculateMonthlyPayment(decimal amount, decimal annualRate, int termMonths)
    {
        if (annualRate == 0) return amount / termMonths;
        var r = (double)(annualRate / 100 / 12);
        var n = termMonths;
        var pmt = (double)amount * r * Math.Pow(1 + r, n) / (Math.Pow(1 + r, n) - 1);
        return Math.Round((decimal)pmt, 2);
    }

    private static List<LoanSchedule> GenerateSchedule(int loanId, decimal pmt, int termMonths)
    {
        var schedule = new List<LoanSchedule>();
        var startDate = DateTime.UtcNow;
        for (int i = 1; i <= termMonths; i++)
        {
            schedule.Add(new LoanSchedule
            {
                LoanId = loanId,
                PMT = pmt,
                Date = startDate.AddMonths(i)
            });
        }
        return schedule;
    }

    private static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }

    private static LoanResponseDto MapToResponse(Loan loan) => new(
        loan.Id, loan.CustomerId, loan.Amount, loan.InterestRate,
        loan.TermMonths, loan.MonthlyPayment, loan.Status.ToString(), loan.CreatedAt);
}