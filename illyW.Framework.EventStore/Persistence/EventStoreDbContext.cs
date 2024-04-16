using illyW.Framework.EventStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace illyW.Framework.EventStore.Persistence
{
    public class EventStoreDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<StreamCheckpoint> StreamCheckpoints { get; set; }
        public DbSet<AllCheckpoint> AllCheckpoints { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Checkpoint>();
            
            modelBuilder.Entity<Checkpoint>()
                .HasDiscriminator<string>("RehydratorType")
                .HasValue<Checkpoint>("Basic")
                .HasValue<StreamCheckpoint>("Stream")
                .HasValue<AllCheckpoint>("All");

            base.OnModelCreating(modelBuilder);
        }
    }
}
