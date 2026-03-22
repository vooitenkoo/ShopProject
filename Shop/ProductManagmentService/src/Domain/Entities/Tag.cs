using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Tag {

    [Key]
    public Guid TagId {get; set;}
    public string TagName {get; set;}
    public List<Product>? Products {get;} = [];
}