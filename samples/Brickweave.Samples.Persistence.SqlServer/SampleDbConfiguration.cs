using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.Persistence.SqlServer
{
    public class SampleDbConfiguration
    {
        public string ConnectionString { get; set; }

        public DbContextOptions<SampleDbContext> BuildOptions()
        {
            return new DbContextOptionsBuilder<SampleDbContext>()
                .UseSqlServer(ConnectionString)
                .Options;
        }
    }
}
