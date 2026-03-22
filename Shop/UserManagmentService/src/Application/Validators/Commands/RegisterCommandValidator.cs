using FluentValidation;
using Application.Features.Authentication.Commands.Register;

namespace Application.Validators.Commands;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.RegisterUserDto)
            .NotNull().WithMessage("Registration data is required")
            .SetValidator(new DTOs.RegisterUserDTOValidator());
    }
} 