using FluentAssertions;
using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using LoanManagement.Application.Services;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using Moq;

namespace LoanManagement.Tests.Services;

public class LoanServiceTests
{
    private readonly Mock<ILoanRepository> _loanRepoMock = new();
    private readonly Mock<ICustomerRepository> _customerRepoMock = new();
    private readonly LoanService _sut;

    public LoanServiceTests()
    {
        _sut = new LoanService(_loanRepoMock.Object, _customerRepoMock.Object);
    }

    [Fact]
    public async Task CreateApplication_WhenCreditScoreLow_ShouldRejectLoan()
    {
        var customer = new Customer
        {
            Id = 1,
            FirstName = "გიორგი",
            LastName = "ბერიძე",
            BirthDate = DateTime.Today.AddYears(-25),
            CreditScore = 200
        };

        _customerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);
        _loanRepoMock.Setup(r => r.AddAsync(It.IsAny<Loan>()))
            .ReturnsAsync((Loan l) => { l.Id = 1; return l; });

        var dto = new CreateLoanApplicationDto(1, 5000, 12, 24);
        var result = await _sut.CreateApplicationAsync(dto);

        result.Status.Should().Be("Rejected");
    }

    [Fact]
    public async Task CreateApplication_WhenCreditScoreHigh_ShouldApproveLoan()
    {
        var customer = new Customer
        {
            Id = 1,
            FirstName = "ნინო",
            LastName = "კვარაცხელია",
            BirthDate = DateTime.Today.AddYears(-30),
            CreditScore = 700
        };

        _customerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);
        _loanRepoMock.Setup(r => r.AddAsync(It.IsAny<Loan>()))
            .ReturnsAsync((Loan l) => { l.Id = 1; return l; });
        _loanRepoMock.Setup(r => r.AddScheduleAsync(It.IsAny<IEnumerable<LoanSchedule>>()))
            .Returns(Task.CompletedTask);

        var dto = new CreateLoanApplicationDto(1, 5000, 12, 24);
        var result = await _sut.CreateApplicationAsync(dto);

        result.Status.Should().Be("Approved");
    }

    [Fact]
    public async Task CreateApplication_WhenCustomerNotFound_ShouldThrow()
    {
        _customerRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Customer?)null);

        var dto = new CreateLoanApplicationDto(99, 5000, 12, 24);
        await _sut.Invoking(s => s.CreateApplicationAsync(dto))
            .Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task CreateApplication_WhenAgeUnder18_ShouldThrow()
    {
        var customer = new Customer
        {
            Id = 1,
            BirthDate = DateTime.Today.AddYears(-16),
            CreditScore = 700
        };

        _customerRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);

        var dto = new CreateLoanApplicationDto(1, 5000, 12, 24);
        await _sut.Invoking(s => s.CreateApplicationAsync(dto))
            .Should().ThrowAsync<InvalidOperationException>();
    }
}