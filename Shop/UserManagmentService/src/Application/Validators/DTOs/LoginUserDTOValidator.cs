using FluentValidation;
using Shared.DTOs;

namespace Application.Validators.DTOs;

public class LoginUserDTOValidator : AbstractValidator<LoginUserDTO>
{
    public LoginUserDTOValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.UserPassword)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
    }
} 