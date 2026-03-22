using System.IdentityModel.Tokens.Jwt;
using Application.IRepositories;
using Application.IServices;
using Domain.Entities;
using Shared.DTOs;
using Shared.Enums;
using DotNetEnv;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Infrastrucure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;
    public AuthenticationService(IUserRepository repository, IConfiguration configuration) {
        _repository = repository;
        _configuration = configuration;
    }
    public async Task<string> AuthenticateUser(LoginUserDTO loginUser)
    {
        Env.Load();
        var user = await _repository.GetByName(loginUser.UserName);
        if (user is null || !BCrypt.Net.BCrypt.Verify(loginUser.UserPassword, user.PasswordHash))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor {

            Subject = new ClaimsIdentity(new Claim[]{

                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.UserRole is Shared.Enums.Role.Admin ? "Admin" : "Client"),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())

            }),

            Expires = DateTime.Now.AddHours(2),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

        };

        user.LastLogin = DateTime.Now;
        await _repository.UpdateUser(user.UserId, user);
        await _repository.SaveChanges();

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<bool> RegistrateUser(RegisterUserDTO registerUser)
    {
        if (registerUser.UserPassword != registerUser.UserVerifyPassword)
            return false;

        if (await _repository.AnyAsync(registerUser.UserName, registerUser.UserEmail))
            return false;

        var user = new User {
            UserName = registerUser.UserName,
            UserEmail = registerUser.UserEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerUser.UserPassword),
            UserRole = registerUser.UserRole,
            CreatedAt = DateTime.Now,
            LastLogin = DateTime.Now
        };

        await _repository.CreateUser(user);
        await _repository.SaveChanges();

        return true;
        
    }
}