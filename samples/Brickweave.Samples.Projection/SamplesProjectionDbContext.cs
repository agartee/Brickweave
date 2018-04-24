using Brickweave.Samples.Projection.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.Projection
{
    public class SamplesProjectionDbContext : DbContext
    {
        public DbSet<PersonProjection> PersonProjection { get; set; }

        public SamplesProjectionDbContext(DbContextOptions<SamplesProjectionDbContext> options) :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonProjection>(p => { p.HasKey(k => k.Id); });

            base.OnModelCreating(modelBuilder);
        }
    }
}
