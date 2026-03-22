using Shared.DTOs;

namespace Application.IServices;

public interface IAuthenticationService {

    Task<string> AuthenticateUser(LoginUserDTO loginUser);
    Task<bool> RegistrateUser(RegisterUserDTO registerUser);

}