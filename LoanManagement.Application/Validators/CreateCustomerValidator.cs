using FluentValidation;
using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Validators;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("სახელი სავალდებულოა.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("გვარი სავალდებულოა.");
        RuleFor(x => x.PersonalNumber).NotEmpty().WithMessage("პირადი ნომერი სავალდებულოა.");
        RuleFor(x => x.BirthDate)
            .Must(b => DateTime.Today.Year - b.Year >= 18)
            .WithMessage("მომხმარებელი 18 წელს მიღწეული უნდა იყოს.");
        RuleFor(x => x.CreditScore).InclusiveBetween(0, 850).WithMessage("CreditScore 0-850 შორის.");
    }
}