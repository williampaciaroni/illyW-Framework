using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace illyW.Framework.Tests.Shared.Fixtures;

public class BaseFeatureFixture<TContext> : IDisposable
where TContext : DbContext
{
    private readonly SqliteConnection _dbConnection;
    private readonly DbContextOptions<TContext> _contextOptions;

    protected BaseFeatureFixture()
    {
        // This will create an in memory SQLite database.
        // The database exists as long the connection is open.
        _dbConnection = new SqliteConnection("Data Source=InMemoryEventsDbContext;Mode=Memory;foreign keys=false");
        _dbConnection.Open();

        _contextOptions = new DbContextOptionsBuilder<TContext>()
            .UseSqlite(_dbConnection)
            .Options;

        // Create the schema within the database
        using var dbContext = NewDbContext();
        dbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected TContext NewDbContext()
    {
        return (TContext)Activator.CreateInstance(typeof(TContext), _contextOptions)!;
    }

    protected IDbContextFactory<TContext> NewDbContextFactory()
    {
        var dbContext = NewDbContext();
        return new TestDbContextFactory(dbContext);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbConnection.Close();
        }
    }
    
    private sealed class TestDbContextFactory(TContext context) : IDbContextFactory<TContext>
    {
        public TContext CreateDbContext()
        {
            return context;
        }
    }
}
