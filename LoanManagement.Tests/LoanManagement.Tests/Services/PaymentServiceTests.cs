using FluentAssertions;
using LoanManagement.Application.DTOs;
using LoanManagement.Application.Interfaces;
using LoanManagement.Application.Services;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Enums;
using Moq;

namespace LoanManagement.Tests.Services;

public class PaymentServiceTests
{
    private readonly Mock<IPaymentRepository> _paymentRepoMock = new();
    private readonly Mock<ILoanRepository> _loanRepoMock = new();
    private readonly PaymentService _sut;

    public PaymentServiceTests()
    {
        _sut = new PaymentService(_paymentRepoMock.Object, _loanRepoMock.Object);
    }

    [Fact]
    public async Task MakePayment_WhenLoanNotFound_ShouldThrow()
    {
        _loanRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Loan?)null);

        var dto = new CreatePaymentDto(99, 500);
        await _sut.Invoking(s => s.MakePaymentAsync(dto))
            .Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task MakePayment_WhenLoanIsClosed_ShouldThrow()
    {
        var loan = new Loan { Id = 1, Status = LoanStatus.Closed, MonthlyPayment = 200, TermMonths = 12 };
        _loanRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(loan);

        var dto = new CreatePaymentDto(1, 200);
        await _sut.Invoking(s => s.MakePaymentAsync(dto))
            .Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task MakePayment_WhenLoanIsPending_ShouldThrow()
    {
        var loan = new Loan { Id = 1, Status = LoanStatus.Pending, MonthlyPayment = 200, TermMonths = 12 };
        _loanRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(loan);

        var dto = new CreatePaymentDto(1, 200);
        await _sut.Invoking(s => s.MakePaymentAsync(dto))
            .Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task MakePayment_WhenValidPayment_ShouldReturnPaymentResponse()
    {
        var loan = new Loan { Id = 1, Status = LoanStatus.Approved, MonthlyPayment = 200, TermMonths = 12 };
        var payment = new Payment { Id = 1, LoanId = 1, Amount = 200, PaymentDate = DateTime.UtcNow };

        _loanRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(loan);
        _paymentRepoMock.Setup(r => r.AddAsync(It.IsAny<Payment>())).ReturnsAsync(payment);
        _paymentRepoMock.Setup(r => r.GetByLoanIdAsync(1)).ReturnsAsync(new List<Payment> { payment });

        var dto = new CreatePaymentDto(1, 200);
        var result = await _sut.MakePaymentAsync(dto);

        result.Should().NotBeNull();
        result.LoanId.Should().Be(1);
        result.Amount.Should().Be(200);
    }

    [Fact]
    public async Task MakePayment_WhenFullyPaid_ShouldCloseLoan()
    {
        var loan = new Loan { Id = 1, Status = LoanStatus.Approved, MonthlyPayment = 500, TermMonths = 2 };
        var existing = new Payment { Id = 1, LoanId = 1, Amount = 500 };
        var newPayment = new Payment { Id = 2, LoanId = 1, Amount = 500, PaymentDate = DateTime.UtcNow };

        _loanRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(loan);
        _paymentRepoMock.Setup(r => r.AddAsync(It.IsAny<Payment>())).ReturnsAsync(newPayment);
        _paymentRepoMock.Setup(r => r.GetByLoanIdAsync(1))
            .ReturnsAsync(new List<Payment> { existing, newPayment });
        _loanRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Loan>())).Returns(Task.CompletedTask);

        var dto = new CreatePaymentDto(1, 500);
        await _sut.MakePaymentAsync(dto);

        _loanRepoMock.Verify(r => r.UpdateAsync(It.Is<Loan>(l => l.Status == LoanStatus.Closed)), Times.Once);
    }
}