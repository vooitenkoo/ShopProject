using System;
using FluentValidation;
using Application.Features.Users.Commands.DeleteUser;

namespace Application.Validators.Commands;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User Id is required")
            .NotEqual(Guid.Empty).WithMessage("User Id must be a valid GUID");
    }
} 