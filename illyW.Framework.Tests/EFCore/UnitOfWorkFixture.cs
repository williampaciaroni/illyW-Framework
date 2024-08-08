using FluentAssertions;
using illyW.Framework.Core.RepositoryPattern;
using illyW.Framework.EFCore;
using illyW.Framework.Tests.EFCore.Shared;
using illyW.Framework.Tests.Shared.Attributes;
using illyW.Framework.Tests.Shared.Fixtures;

namespace illyW.Framework.Tests.EFCore;

public class UnitOfWorkFixture : BaseFeatureFixture<TestCoreDbContext>
{
    [Fact]
    public async Task TestUnitOfWork_Transaction_NoCommit()
    {
        var context = NewDbContext();
        var repository = CreateRepository(context);

        using( var uow = CreateUoW(context, new[] { repository }))
        {
            var testRepository = uow.GetRepository<ITestRepository>();

            await testRepository.AddAsync(new TestEntity() { Id = 1, Value = "Test" });
        }
        
        var context2 = NewDbContext();

        context2.TestEntities.Count().Should().Be(0);
    }
    
    [Fact]
    public async Task TestUnitOfWork_Transaction_Commit()
    {
        var context = NewDbContext();
        var repository = CreateRepository(context);

        using( var uow = CreateUoW(context, new[] { repository }))
        {
            var testRepository = uow.GetRepository<ITestRepository>();

            await testRepository.AddAsync(new TestEntity() { Id = 1, Value = "Test" });
            
            uow.Commit();
        }
        
        var context2 = NewDbContext();

        context2.TestEntities.Count().Should().Be(1);
    }
    
    [Fact]
    public async Task TestUnitOfWork_Transaction_Rollback()
    {
        var context = NewDbContext();
        var repository = CreateRepository(context);

        using var uow = CreateUoW(context, new[] { repository });
        var testRepository = uow.GetRepository<ITestRepository>();

        await testRepository.AddAsync(new TestEntity() { Id = 1, Value = "Test" });
            
        uow.Rollback();

        testRepository.Fetch().Count().Should().Be(0);
    }
    
    [Fact]
    public async Task TestUnitOfWork_Transaction_RejectScalarChanges_Added()
    {
        var context = NewDbContext();
        var repository = CreateRepository(context);

        using var uow = CreateUoW(context, new[] { repository });
        var testRepository = uow.GetRepository<ITestRepository>();
        await testRepository.AddAsync(new TestEntity() { Id = 1, Value = "Test" });
        uow.BeginTransaction();

        context.TestEntities.Count().Should().Be(0);
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestUnitOfWork_Transaction_RejectScalarChanges_Modified(TestEntity testEntity)
    {
        var repository = CreateRepository(NewDbContext());
        testEntity.Value = "Value";
        await repository.AddAsync(testEntity);

        var context = NewDbContext();

        using var uow = CreateUoW(context, new[] { repository });
        var testRepository = uow.GetRepository<ITestRepository>();
        var entity = await testRepository.GetSingleAsync(testEntity.Id);
        entity.Value = "NewValue";
        await testRepository.UpdateAsync(entity);
        
        uow.BeginTransaction();

        context.TestEntities.Count().Should().Be(1);
        context.TestEntities.First().Value.Should().Be("Value");
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestUnitOfWork_Transaction_RejectScalarChanges_Deleted(TestEntity testEntity)
    {
        var repository = CreateRepository(NewDbContext());
        await repository.AddAsync(testEntity);

        var context = NewDbContext();

        using var uow = CreateUoW(context, new[] { repository });
        var testRepository = uow.GetRepository<ITestRepository>();
        await testRepository.DeleteAsync(await testRepository.GetSingleAsync(testEntity.Id));
        
        uow.BeginTransaction();

        context.TestEntities.Count().Should().Be(1);
    }
    
    private static TestRepository CreateRepository(TestCoreDbContext context)
    {
        return new TestRepository(context);
    }
    
    private static IUnitOfWork CreateUoW(TestCoreDbContext context, IEnumerable<IGenericRepository> repositories)
    {
        return new UnitOfWork<TestCoreDbContext>(context, repositories);
    }
}
