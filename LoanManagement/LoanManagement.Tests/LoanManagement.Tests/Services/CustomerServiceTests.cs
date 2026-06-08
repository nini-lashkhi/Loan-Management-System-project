using FluentAssertions;
using LoanManagement.Application.Interfaces;
using LoanManagement.Application.Services;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using Moq;

namespace LoanManagement.Tests.Services;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _customerRepoMock = new();
    private readonly CustomerService _sut;

    public CustomerServiceTests()
    {
        _sut = new CustomerService(_customerRepoMock.Object);
    }

    [Fact]
    public async Task GetCustomerLoans_WhenCustomerNotFound_ShouldThrow()
    {
        _customerRepoMock.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        await _sut.Invoking(s => s.GetCustomerLoansAsync(99))
            .Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GetCustomerLoans_WhenCustomerExists_ShouldReturnLoans()
    {
        var loans = new List<Loan>
        {
            new() { Id = 1, CustomerId = 1, Amount = 5000, InterestRate = 12,
                    TermMonths = 24, MonthlyPayment = 235.37m,
                    Status = LoanStatus.Approved, CreatedAt = DateTime.UtcNow },
            new() { Id = 2, CustomerId = 1, Amount = 3000, InterestRate = 10,
                    TermMonths = 12, MonthlyPayment = 263.74m,
                    Status = LoanStatus.Closed, CreatedAt = DateTime.UtcNow }
        };

        _customerRepoMock.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _customerRepoMock.Setup(r => r.GetLoansByCustomerIdAsync(1)).ReturnsAsync(loans);

        var result = await _sut.GetCustomerLoansAsync(1);

        result.Should().HaveCount(2);
        result.Should().Contain(l => l.Status == "Approved");
        result.Should().Contain(l => l.Status == "Closed");
    }

    [Fact]
    public async Task GetCustomerLoans_WhenNoLoans_ShouldReturnEmptyList()
    {
        _customerRepoMock.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _customerRepoMock.Setup(r => r.GetLoansByCustomerIdAsync(1))
            .ReturnsAsync(new List<Loan>());

        var result = await _sut.GetCustomerLoansAsync(1);

        result.Should().BeEmpty();
    }
}