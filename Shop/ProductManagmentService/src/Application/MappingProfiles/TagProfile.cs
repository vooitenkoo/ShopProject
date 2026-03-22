using AutoMapper;
using Domain.Entities;
using Shared.DTOs;

namespace Application.MappingProfiles;

public class TagProfile : Profile {

    public TagProfile() {

        CreateMap<Tag, TagDTO>();
        CreateMap<TagDTO, Tag>();

    }

} 