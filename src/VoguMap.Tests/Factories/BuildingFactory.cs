using VoguMap.Domain.Entities;

namespace VoguMap.Tests.Factories
{
    public static class BuildingFactory
    {
        public static Building Create()
        {
            return new Building
            {
                Name = "Building",
                Address = "Address"
            };
        }

        public static Building Create(string? name = null, string? address = null)
        {
            return new Building
            {
                Name = name ?? "Building",
                Address = address ?? "Address"
            };
        }
    }
}