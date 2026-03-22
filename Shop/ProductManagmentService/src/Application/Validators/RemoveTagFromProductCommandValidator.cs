using Application.Products.Commands;
using FluentValidation;

namespace Application.Products.Validators;

public class RemoveTagFromProductCommandValidator : AbstractValidator<RemoveTagFromProductCommand>
{
    public RemoveTagFromProductCommandValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("Client ID is required");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.TagId)
            .NotEmpty()
            .WithMessage("Tag ID is required");
    }
} 