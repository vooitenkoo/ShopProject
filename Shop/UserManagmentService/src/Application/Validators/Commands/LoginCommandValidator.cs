using FluentValidation;
using Application.Features.Authentication.Commands.Login;

namespace Application.Validators.Commands;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.LoginUserDto)
            .NotNull().WithMessage("Login data is required")
            .SetValidator(new DTOs.LoginUserDTOValidator());
    }
} 