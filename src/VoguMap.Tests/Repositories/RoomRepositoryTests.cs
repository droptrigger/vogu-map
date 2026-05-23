using Microsoft.EntityFrameworkCore;
using VoguMap.Domain.Entities;
using VoguMap.Infrastructure.Persistence.Context;
using VoguMap.Infrastructure.Persistence.Repositories;
using VoguMap.Tests.Context;
using VoguMap.Tests.Factories;

namespace VoguMap.Tests.Repositories
{
    public class RoomRepositoryTests
    {
        private async Task<Building> SeedBuildingAsync(VoguMapContext context)
        {
            var entity = await context.Buildings.AddAsync(BuildingFactory.Create());
            await context.SaveChangesAsync();
            return entity.Entity;
        }

        [Fact]
        public async Task CreateAsync_AddsRoom()
        {
            var context = TestDbContextFactory.CreateContext();
            var repository = new RoomRepository(context);
            var building = await SeedBuildingAsync(context);

            var name = "Room";
            var floor = 1;

            var result = await repository.CreateAsync(RoomFactory.Create(building.Id, name, floor));

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal(name, result.Name);
            Assert.Equal(floor, result.Floor);
            Assert.Equal(1, await context.Rooms.CountAsync());
        }

        [Fact]
        public async Task UpdateAsync_ChangesRoomName()
        {
            var context = TestDbContextFactory.CreateContext();
            var repository = new RoomRepository(context);
            var building = await SeedBuildingAsync(context);

            var name = "Room";
            var newName = "New name";

            var created = await context.Rooms.AddAsync(RoomFactory.Create(building.Id, name));
            await context.SaveChangesAsync();
            var room = await repository.GetByIdForUpdateAsync(created.Entity.Id);

            Assert.NotNull(room);

            room.Name = newName;
            var result = await repository.UpdateAsync(room);

            Assert.Equal(newName, result.Name);
        }

        [Fact]
        public async Task DeleteAsync_DeletesRoom()
        {
            var context = TestDbContextFactory.CreateContext();
            var repository = new RoomRepository(context);
            var building = await SeedBuildingAsync(context);

            var created = await context.AddAsync(RoomFactory.Create(building.Id));
            await context.SaveChangesAsync();
            var room = await repository.GetByIdForUpdateAsync(created.Entity.Id);

            Assert.NotNull(room);
            await repository.DeleteAsync(room);

            Assert.Null(await context.Rooms.FirstOrDefaultAsync());
        }

        [Fact]
        public async Task DeleteAsync_ThrowsWhenNoTracking()
        {
            var context = TestDbContextFactory.CreateContext();
            var repository = new RoomRepository(context);
            var building = await SeedBuildingAsync(context);

            var created = await repository.CreateAsync(RoomFactory.Create(building.Id));

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await repository.DeleteAsync(created);
            });
        }

        [Fact]
        public async Task ExistsByIdAsync_Exists()
        {
            var context = TestDbContextFactory.CreateContext();
            var repository = new RoomRepository(context);
            var building = await SeedBuildingAsync(context);

            var room = await context.Rooms.AddAsync(RoomFactory.Create(building.Id));
            await context.SaveChangesAsync();

            Assert.True(await repository.ExistsByIdAsync(room.Entity.Id));
        }

        [Fact]
        public async Task GetByIdAsync_HasBuilding()
        {
            var context = TestDbContextFactory.CreateContext();
            var repository = new RoomRepository(context);
            var building = await SeedBuildingAsync(context);

            var created = await context.Rooms.AddAsync(RoomFactory.Create(building.Id));
            await context.SaveChangesAsync();

            var room = await repository.GetByIdAsync(created.Entity.Id);

            Assert.NotNull(room);
            Assert.NotNull(room.Building);
        }
    }
}