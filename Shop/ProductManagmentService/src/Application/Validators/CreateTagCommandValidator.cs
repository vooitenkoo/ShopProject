using Application.Tags.Commands;
using FluentValidation;

namespace Application.Tags.Validators;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
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