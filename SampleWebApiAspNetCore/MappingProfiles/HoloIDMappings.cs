using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class HoloIDMappings : Profile
    {
        public HoloIDMappings()
        {
            CreateMap<HoloIDEntity, HoloIDDto>().ReverseMap();
            CreateMap<HoloIDEntity, HoloIDUpdateDto>().ReverseMap();
            CreateMap<HoloIDEntity, HoloIDCreateDto>().ReverseMap();
        }
    }
}
