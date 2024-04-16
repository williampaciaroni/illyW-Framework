using FluentAssertions;
using illyW.Framework.Core.RepositoryPattern;
using illyW.Framework.EFCore.Extensions;
using illyW.Framework.Tests.EFCore.Shared;
using illyW.Framework.Tests.Shared.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace illyW.Framework.Tests.EFCore;

public class ExtensionFixture
{
    [Theory]
    [DefaultAutoData]
    public void AddRepository_Success(ServiceCollection serviceCollection)
    {
        serviceCollection.AddRepository<ITestRepository, TestRepository>();

        serviceCollection.Any(x => x.ServiceType == typeof(ITestRepository)).Should().BeTrue();
        serviceCollection.Any(x => x.ServiceType == typeof(IGenericRepository)).Should().BeTrue();
    }
    
    [Theory]
    [DefaultAutoData]
    public void AddUnitOfWork_Success(ServiceCollection serviceCollection)
    {
        serviceCollection.AddUnitOfWork<TestCoreDbContext>();

        serviceCollection.Any(x => x.ServiceType == typeof(IUnitOfWork)).Should().BeTrue();
    }
    
}