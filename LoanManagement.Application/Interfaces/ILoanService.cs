using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Interfaces;

public interface ILoanService
{
    Task<LoanResponseDto> CreateApplicationAsync(CreateLoanApplicationDto dto);
    Task<LoanResponseDto> GetByIdAsync(int id);
}