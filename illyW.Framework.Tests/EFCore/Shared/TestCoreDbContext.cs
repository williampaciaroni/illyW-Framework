using Microsoft.EntityFrameworkCore;

namespace illyW.Framework.Tests.EFCore.Shared;

public class TestCoreDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<TestEntity> TestEntities { get; set; } = null!;
}