
using AutoMapper;
using OnTwitter.Application.Models;
using OnTwitter.Domain.Entities;

namespace OnTwitter.Application.Common.Mappings;

public class MapperProfile: Profile
{
	public MapperProfile()
	{
		CreateMap<TwitterDto, Twitter>().ReverseMap();
        CreateMap<TwitterHashTagDto, TwitterHashTag>().ReverseMap();
    }
}
