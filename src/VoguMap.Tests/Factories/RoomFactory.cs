using VoguMap.Domain.Entities;

namespace VoguMap.Tests.Factories
{
    public static class RoomFactory
    {
        public static Room Create(int buildingId, string? name = null, int? floor = null)
        {
            return new Room
            {
                BuildingId = buildingId,
                Name = name ?? "Room",
                Floor = floor ?? 1
            };
        }
    }
}