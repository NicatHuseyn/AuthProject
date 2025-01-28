using AuthProject.Core.DTOs;
using FluentValidation;

namespace AuthProject.API.Validations.UserValidators;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x=>x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email must be required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password must be required");

        RuleFor(x=>x.UserName)
            .NotEmpty()
            .WithMessage("Username must be required");
    }
}
