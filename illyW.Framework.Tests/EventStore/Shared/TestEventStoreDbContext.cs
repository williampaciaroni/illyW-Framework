using illyW.Framework.EventStore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace illyW.Framework.Tests.EventStore.Shared;

public class TestEventStoreDbContext(DbContextOptions options) : EventStoreDbContext(options)
{
    
}