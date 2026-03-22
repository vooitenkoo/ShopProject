using FluentValidation;
using Application.Features.Users.Commands.UpdateUser;

namespace Application.Validators.Commands;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User Id is required");

        RuleFor(x => x.UpdateUserDto)
            .NotNull().WithMessage("Update user data is required")
            .SetValidator(new DTOs.UpdateUserDTOValidator());
    }
} 