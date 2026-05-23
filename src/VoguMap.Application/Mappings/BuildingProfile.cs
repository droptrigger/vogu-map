using AutoMapper;
using VoguMap.Application.DTOs.Building;
using VoguMap.Domain.Entities;

namespace VoguMap.Application.Mappings
{
    public class BuildingProfile : Profile
    {
        public BuildingProfile()
        {
            CreateMap<Building, BuildingGetDto>();
            CreateMap<Building, BuildingBriefDto>();
        }
    }
}