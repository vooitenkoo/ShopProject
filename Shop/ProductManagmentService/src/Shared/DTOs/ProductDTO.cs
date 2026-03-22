namespace Shared.DTOs;

public record class ProductDTO (
    Guid? ProductId,
    string ProductName,
    decimal ProductPrice,
    string ProductDescription
);