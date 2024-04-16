using illyW.Framework.Core.RepositoryPattern;

namespace illyW.Framework.Tests.EFCore.Shared;

public interface ITestRepository : IGenericRepository<TestEntity, int>
{
    
}