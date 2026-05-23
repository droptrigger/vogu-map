using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VoguMap.Application.DTOs.Building;
using VoguMap.Application.Services.Implementations;
using VoguMap.Domain.Entities;
using VoguMap.Infrastructure.Persistence.Context;
using VoguMap.Infrastructure.Persistence.Repositories;
using VoguMap.Tests.Context;

namespace VoguMap.Tests.IntegrationTests
{
    public class BuildingServiceIntegrationTests : IAsyncLifetime
    {
        private VoguMapContext _context = null!;
        private BuildingService _service = null!;
        private IMapper _mapper = null!;

        public async Task InitializeAsync()
        {
            _context = TestDbContextFactory.CreateSqliteContext();

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Building, BuildingGetDto>();
                },
                Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance
            );
            _mapper = config.CreateMapper();

            var repository = new BuildingRepository(_context);
            _service = new BuildingService(repository, _mapper);

            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddBuildingToDatabase_AndReturnMappedDto()
        {
            // Arrange
            var createDto = new BuildingCreateDto
            {
                Name = "Integration Building",
                Address = "123 SQLite Street",
                Longitude = 12.34m,
                Latitude = 56.78m
            };

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.True(result.Id > 0);
            Assert.Equal(createDto.Name, result.Name);
            Assert.Equal(createDto.Address, result.Address);
            Assert.Equal(createDto.Longitude, result.Longitude);
            Assert.Equal(createDto.Latitude, result.Latitude);

            var savedEntity = await _context.Buildings.AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == result.Id);
            Assert.NotNull(savedEntity);
            Assert.Equal(createDto.Name, savedEntity.Name);
            Assert.Equal(createDto.Address, savedEntity.Address);
            Assert.Equal(createDto.Longitude, savedEntity.Longitude);
            Assert.Equal(createDto.Latitude, savedEntity.Latitude);
        }
    }
}