using AutoMapper;
using VoguMap.Application.DTOs.Room;
using VoguMap.Domain.Entities;

namespace VoguMap.Application.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomGetDto>();
        }
    }
}