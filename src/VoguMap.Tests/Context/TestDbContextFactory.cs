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
    }
}