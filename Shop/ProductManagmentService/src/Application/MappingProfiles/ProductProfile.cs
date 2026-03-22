using AutoMapper;
using Domain.Entities;
using Shared.DTOs;

namespace Application.MappingProfiles;

public class ProductProfile : Profile {

    public ProductProfile() {

        CreateMap<Product, ProductDTO>();
        CreateMap<ProductDTO, Product>();

    }

}