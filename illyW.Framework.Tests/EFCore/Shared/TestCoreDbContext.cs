using Microsoft.EntityFrameworkCore;

namespace illyW.Framework.Tests.EFCore.Shared;

public class TestCoreDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<TestEntity> TestEntities { get; set; } = null!;
    public DbSet<TestEntity2> TestEntities2 { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestEntity>(e =>
        {
            e.HasOne(x => x.TestEntity2)
                .WithOne()
                .HasForeignKey<TestEntity>(x => x.TestEntity2Id);
        });
        base.OnModelCreating(modelBuilder);
    }
}