using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Brickweave.Samples.Persistence.SqlServer
{
    public class SampleDbConfiguration
    {
        public string ConnectionString { get; set; }

        public DbContextOptions<SampleDbContext> BuildOptions()
        {
            return new DbContextOptionsBuilder<SampleDbContext>()
                .UseSqlServer(ConnectionString)
                .ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning))
                .Options;
        }
    }
}
