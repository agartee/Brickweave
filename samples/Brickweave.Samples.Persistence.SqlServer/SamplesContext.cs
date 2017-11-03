using System;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.Persistence.SqlServer
{
    public class SamplesContext : DbContext
    {
        public SamplesContext(DbContextOptions<SamplesContext> options) : base(options)
        {
        }

        public DbSet<PersonData> Persons { get; set; }
    }

    public class PersonData
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
