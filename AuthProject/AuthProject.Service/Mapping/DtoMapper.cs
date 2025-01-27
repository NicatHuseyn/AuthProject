using AuthProject.Core.DTOs;
using AuthProject.Core.Entities;
using AutoMapper;

namespace AuthProject.Service.Mapping;

public class DtoMapper:Profile
{
    public DtoMapper()
    {
        CreateMap<ProductDto,Product>().ReverseMap();
        CreateMap<AppUserDto,AppUser>().ReverseMap();
    }
}
