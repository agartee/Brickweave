using Brickweave.Samples.Persistence.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.Persistence.SqlServer
{
    public class SamplesContext : DbContext
    {
        public SamplesContext(DbContextOptions<SamplesContext> options) : base(options)
        {
        }

        public DbSet<PersonSnapshot> Persons { get; set; }
    }
}
