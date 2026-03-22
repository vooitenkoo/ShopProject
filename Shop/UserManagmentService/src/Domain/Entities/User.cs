using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Domain.Entities;

public class User {

    [Key]
    public Guid UserId {get; set;}
    public string? UserName {get; set;}
    public string? UserEmail {get; set;}
    public string? PasswordHash {get; set;}
    public Role UserRole {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime LastLogin {get; set;}

}