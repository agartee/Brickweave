using Brickweave.EventStore.SqlServer.Entities;
using Brickweave.Messaging.SqlServer.Entities;
using EventSourcingDemo.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSourcingDemo.SqlServer
{
    public class EventSourcingDemoDbContext : DbContext
    {
        public const string SCHEMA_NAME = "EventSourcingDemo";

        public EventSourcingDemoDbContext(DbContextOptions<EventSourcingDemoDbContext> options) : base(options)
        {

        }

        public DbSet<PersonalAccountData> PersonalAccounts { get; set; }
        public DbSet<BusinessAccountData> BusinessAccounts { get; set; }
        public DbSet<EventData> Events { get; set; }
        public DbSet<PersonData> People { get; set; }
        public DbSet<CompanyData> Companies { get; set; }
        public DbSet<MessageData> MessageOutbox { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(SCHEMA_NAME);

            modelBuilder.Entity<PersonalAccountData>()
                .HasOne(a => a.AccountHolder)
                .WithMany(p => p.Accounts)
                .HasForeignKey(a => a.AccountHolderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BusinessAccountData>()
                .HasOne(a => a.AcountHolder)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.AccountHolderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
