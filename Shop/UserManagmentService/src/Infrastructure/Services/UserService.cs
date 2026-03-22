using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Application.IRepositories;
using Shared.DTOs;

namespace Infrastrucure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    public UserService(IUserRepository repository, IMapper mapper) {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<List<UserDTO>> GetAll()
    {
        return _mapper.Map<List<UserDTO>>(await _repository.GetAll());
    }

    public async Task<UserDTO> GetById(Guid id)
    {
        return _mapper.Map<UserDTO>(await _repository.GetById(id));
    }

    public async Task RemoveUser(Guid id)
    {
        await _repository.RemoveUser(id);
        await _repository.SaveChanges();
    }

    public async Task<UserDTO> UpdateUser(Guid id, UpdateUserDTO updateUser)
    {
        var user = await _repository.GetById(id);
        user.UserName = updateUser.UserName;
        user.UserEmail = updateUser.UserEmail;
        var updatedUser = await _repository.UpdateUser(id, user);
        await _repository.SaveChanges();
        return _mapper.Map<UserDTO>(updatedUser);
    }
}