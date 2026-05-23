using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using VoguMap.Infrastructure.Persistence.Context;

namespace VoguMap.Tests.Context
{
    public static class TestDbContextFactory
    {
        public static VoguMapContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<VoguMapContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new VoguMapContext(options);
        }

        public static VoguMapContext CreateSqliteContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<VoguMapContext>()
                .UseSqlite(connection)
                .Options;
            var context = new VoguMapContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}