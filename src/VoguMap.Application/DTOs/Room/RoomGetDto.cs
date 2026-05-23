using VoguMap.Application.DTOs.Building;

namespace VoguMap.Application.DTOs.Room
{
    public class RoomGetDto
    {
        public int Id { get; set; }
        public BuildingBriefDto Building { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int Floor { get; set; }
    }
}