using FluentValidation;
using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Validators;

public class CreatePaymentValidator : AbstractValidator<CreatePaymentDto>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.LoanId).GreaterThan(0).WithMessage("LoanId სავალდებულოა.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("გადახდის თანხა 0-ზე მეტი უნდა იყოს.");
    }
}