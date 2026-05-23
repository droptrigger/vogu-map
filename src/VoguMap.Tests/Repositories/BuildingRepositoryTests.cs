using Microsoft.EntityFrameworkCore;
using VoguMap.Infrastructure.Persistence.Repositories;
using VoguMap.Tests.Context;
using VoguMap.Tests.Factories;

namespace VoguMap.Tests.Repositories
{
    public class BuildingRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_AddsBuilding()
        {
            await using var context = TestDbContextFactory.CreateContext();
            var repository = new BuildingRepository(context);

            var name = "Building";
            var address = "Address";

            var result = await repository.CreateAsync(BuildingFactory.Create(name, address));

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal(name, result.Name);
            Assert.Equal(address, result.Address);
            Assert.Equal(1, await context.Buildings.CountAsync());
        }

        [Fact]
        public async Task UpdateAsync_ChangesBuildingName()
        {
            await using var context = TestDbContextFactory.CreateContext();
            var repository = new BuildingRepository(context);

            var created = await context.Buildings.AddAsync(BuildingFactory.Create());
            await context.SaveChangesAsync();
            var building = await repository.GetByIdForUpdateAsync(created.Entity.Id);

            Assert.NotNull(building);

            var newName = "New name";

            building.Name = newName;
            var result = await repository.UpdateAsync(building);

            Assert.Equal(newName, result.Name);
        }

        [Fact]
        public async Task DeleteAsync_DeletesBuilding()
        {
            await using var context = TestDbContextFactory.CreateContext();
            var repository = new BuildingRepository(context);

            var building = await context.Buildings.AddAsync(BuildingFactory.Create());
            await context.SaveChangesAsync();
            var entity = await repository.GetByIdForUpdateAsync(building.Entity.Id);

            Assert.NotNull(entity);

            await repository.DeleteAsync(entity);

            Assert.Null(await context.Buildings.FirstOrDefaultAsync());
        }

        [Fact]
        public async Task DeleteAsync_ThrowsWhenNoTracking()
        {
            await using var context = TestDbContextFactory.CreateContext();
            var repository = new BuildingRepository(context);

            var building = await repository.CreateAsync(BuildingFactory.Create());

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await repository.DeleteAsync(building);
            });
        }   
             
        [Fact]
        public async Task ExistsByIdAsync_Exists()
        {
            await using var context = TestDbContextFactory.CreateContext();
            var repository = new BuildingRepository(context);

            var building = await context.Buildings.AddAsync(BuildingFactory.Create());
            await context.SaveChangesAsync();

            Assert.True(await repository.ExistsByIdAsync(building.Entity.Id));
        }
    }
}