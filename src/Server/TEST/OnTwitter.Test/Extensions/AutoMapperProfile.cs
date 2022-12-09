using AutoMapper;
using OnTwitter.Application.Models;
using OnTwitter.Domain.Entities;

namespace OnTwitter.Test;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TwitterDto, Twitter>().ReverseMap();
        CreateMap<TwitterHashTagDto, TwitterHashTag>().ReverseMap();
    }
}