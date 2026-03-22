using Application.Tags.Commands;
using FluentValidation;

namespace Application.Tags.Validators;

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Tag ID is required");

        RuleFor(x => x.Tag)
            .NotNull()
            .WithMessage("Tag is required");

        When(x => x.Tag != null, () =>
        {
            RuleFor(x => x.Tag.TagName)
                .NotEmpty()
                .WithMessage("Tag name is required")
                .MaximumLength(50)
                .WithMessage("Tag name cannot exceed 50 characters");
        });
    }
} 