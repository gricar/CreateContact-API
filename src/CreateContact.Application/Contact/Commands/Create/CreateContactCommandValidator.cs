using FluentValidation;

namespace CreateContact.Application.Contact.Commands.Create;

public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
{
    public CreateContactCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(c => c.DDDCode)
            .NotEmpty().WithMessage("DDD code is required")
            .InclusiveBetween(10, 99)
            .WithMessage("DDDCode must be a valid 2 numeric digits.");

        RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\d{9}$").WithMessage("Phone number must be 9 numeric digits.");

        RuleFor(c => c.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Email must be a valid format.");
    }
}
