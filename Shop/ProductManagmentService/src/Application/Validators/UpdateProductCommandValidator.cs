using Application.Products.Commands;
using FluentValidation;

namespace Application.Products.Validators;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("Client ID is required");

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");

        When(x => x.Product != null, () =>
        {
            RuleFor(x => x.Product.ProductName)
                .NotEmpty()
                .WithMessage("Product name is required")
                .MaximumLength(100)
                .WithMessage("Product name cannot exceed 100 characters");

            RuleFor(x => x.Product.ProductPrice)
                .GreaterThan(0)
                .WithMessage("Product price must be greater than 0");

            RuleFor(x => x.Product.ProductDescription)
                .NotEmpty()
                .WithMessage("Product description is required")
                .MaximumLength(500)
                .WithMessage("Product description cannot exceed 500 characters");
        });
    }
} 