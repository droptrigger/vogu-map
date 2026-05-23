namespace VoguMap.Application.DTOs.Room
{
    public class RoomFilterDto
    {
        public int BuildingId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? Floor { get; set; } = null;
    }
}