using FluentValidation;
using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Validators;

public class CreateLoanApplicationValidator : AbstractValidator<CreateLoanApplicationDto>
{
    public CreateLoanApplicationValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("CustomerId სავალდებულოა.");
        RuleFor(x => x.Amount).InclusiveBetween(500, 50000).WithMessage("თანხა უნდა იყოს 500-დან 50,000-მდე.");
        RuleFor(x => x.InterestRate).GreaterThan(0).LessThanOrEqualTo(100).WithMessage("საპროცენტო განაკვეთი 0-100 შორის.");
        RuleFor(x => x.TermMonths).InclusiveBetween(6, 60).WithMessage("ვადა 6-დან 60 თვემდე.");
    }
}