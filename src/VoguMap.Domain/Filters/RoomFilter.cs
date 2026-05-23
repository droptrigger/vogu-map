namespace VoguMap.Domain.Filters
{
    public sealed record RoomFilter(
        int BuildingId,
        string Name,
        int? Floor = null);
}