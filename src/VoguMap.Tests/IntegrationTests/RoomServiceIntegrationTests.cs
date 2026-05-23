using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VoguMap.Application.DTOs.Building;
using VoguMap.Application.DTOs.Room;
using VoguMap.Application.Services.Implementations;
using VoguMap.Domain.Entities;
using VoguMap.Infrastructure.Persistence.Context;
using VoguMap.Infrastructure.Persistence.Repositories;
using VoguMap.Tests.Context;

namespace VoguMap.Tests.IntegrationTests
{
    public class RoomServiceIntegrationTests : IAsyncLifetime
    {
        private VoguMapContext _context = null!;
        private RoomService _service = null!;
        private IMapper _mapper = null!;

        public async Task InitializeAsync()
        {
            _context = TestDbContextFactory.CreateSqliteContext();

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Room, RoomGetDto>();
                    cfg.CreateMap<Building, BuildingGetDto>();
                    cfg.CreateMap<Building, BuildingBriefDto>();
                },
                Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance
            );
            _mapper = config.CreateMapper();

            var repository = new RoomRepository(_context);
            var buildingRepository = new BuildingRepository(_context);
            _service = new RoomService(repository, buildingRepository, _mapper);

            await SeedDataAsync();
        }

        private async Task SeedDataAsync()
        {
            var building = new Building
            {
                Id = 1,
                Name = "Main Building",
                Address = "123 Main St",
                Longitude = 10.5m,
                Latitude = 20.3m
            };
            _context.Buildings.Add(building);

            _context.Rooms.Add(
                new Room
                {
                    Id = 100,
                    Name = "Conference Room",
                    Description = "Large conference room",
                    BuildingId = 1,
                    Floor = 3
                }
            );
            await _context.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddRoomToDatabase_AndReturnDto()
        {
            // Arrange
            var createDto = new RoomCreateDto
            {
                Name = "New Test Room",
                Description = "Integration test room",
                BuildingId = 1,
                Floor = 4
            };

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.True(result.Id > 0);
            Assert.Equal(createDto.Name, result.Name);
            Assert.Equal(createDto.Description, result.Description);
            Assert.Equal(createDto.BuildingId, result.Building.Id);
            Assert.Equal(createDto.Floor, result.Floor);

            var savedRoom = await _context.Rooms.AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == result.Id);
            Assert.NotNull(savedRoom);
            Assert.Equal(createDto.Name, savedRoom.Name);
        }
    }
}