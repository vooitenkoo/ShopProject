using AutoMapper;
using Domain.Entities;
using Shared.DTOs;

namespace Application.MappingProfles;

public class UserProfile : Profile {

    public UserProfile() {

        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, User>();

    }

}