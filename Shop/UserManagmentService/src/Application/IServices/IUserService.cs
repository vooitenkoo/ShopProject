using Domain.Entities;
using Shared.DTOs;

namespace Application.IServices;

public interface IUserService {
    public Task<List<UserDTO>> GetAll();
    public Task<UserDTO> GetById(Guid id);
    public Task RemoveUser(Guid id);
    public Task<UserDTO> UpdateUser(Guid id, UpdateUserDTO updateUser);
}