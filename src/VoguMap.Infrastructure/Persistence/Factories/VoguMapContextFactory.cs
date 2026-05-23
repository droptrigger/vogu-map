using Microsoft.EntityFrameworkCore.Design;
using VoguMap.Infrastructure.Persistence.Context;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace VoguMap.Infrastructure.Persistence.Factories
{
    public class VoguMapContextFactory : IDesignTimeDbContextFactory<VoguMapContext>
    {
        public VoguMapContext CreateDbContext(string[] args)
        {
            Env.TraversePath().Load();

            var dbHost = Environment.GetEnvironmentVariable("DB_HOST")
                ?? throw new InvalidOperationException("Missing environment variable DB_HOST.");
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT")
                ?? throw new InvalidOperationException("Missing environment variable DB_PORT.");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME")
                ?? throw new InvalidOperationException("Missing environment variable DB_NAME.");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER")
                ?? throw new InvalidOperationException("Missing environment variable DB_USER.");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD")
                ?? throw new InvalidOperationException("Missing environment variable DB_PASSWORD.");

            var connectionString =
                $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

            var optionsBuilder = new DbContextOptionsBuilder<VoguMapContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new VoguMapContext(optionsBuilder.Options);
        }
    }
}