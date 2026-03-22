using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Product {

    [Key]
    public Guid ProductId {get; set;}
    public string ProductName {get; set;}
    public decimal ProductPrice {get; set;}
    public DateTime? ProductCreateTime {get; set;}
    public string ProductDescription {get; set;}
    public List<Tag>? Tags {get;} = [];
    public Guid ClientId {get; set;}

} 