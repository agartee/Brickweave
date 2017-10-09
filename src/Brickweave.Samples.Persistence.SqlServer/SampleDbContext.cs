using Brickweave.Samples.Domain.Persons.Models;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.Persistence.SqlServer
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(SampleDbConfiguration config) 
            : base(config.BuildOptions())
        {
        }
        
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .ToTable("Person")
                .HasKey(p => p.Id);
                
            base.OnModelCreating(modelBuilder);
        }
    }
}
