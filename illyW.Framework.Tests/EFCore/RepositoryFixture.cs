using FluentAssertions;
using illyW.Framework.Tests.EFCore.Shared;
using illyW.Framework.Tests.Shared.Attributes;

namespace illyW.Framework.Tests.EFCore;

public class RepositoryFixture
{
    [Theory]
    [DefaultAutoData]
    public void TestRepository_Add_Success(TestCoreDbContext context, TestEntity entity)
    {
        var repository = CreateRepository(context);
        var r = repository.Add(entity);

        r.IsSuccessful.Should().BeTrue();
        context.TestEntities.SingleOrDefault(x => x.Id == entity.Id).Should().NotBeNull();
        r.Data.Should().BeEquivalentTo(context.TestEntities.SingleOrDefault(x => x.Id == entity.Id));
    }
    
    [Theory]
    [DefaultAutoData]
    public void TestRepository_Add_Null(TestCoreDbContext context)
    {
        var repository = CreateRepository(context);
        var r = repository.Add(null!);

        r.IsSuccessful.Should().BeFalse();
        r.Errors.FirstOrDefault().Should().NotBeNull();
        r.Errors.First().Should().Be($"Entity {typeof(TestEntity).FullName} is null");
    }
    
    [Theory]
    [DefaultAutoData]
    public void TestRepository_Add_Duplicate(TestCoreDbContext context, TestEntity entity)
    {
        var repository = CreateRepository(context);
        repository.Add(entity);

        var r = repository.Add(new TestEntity
        {
            Id = entity.Id
        });

        r.IsSuccessful.Should().BeFalse();
        r.Errors.FirstOrDefault().Should().NotBeNull();
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_AddAsync_Success(TestCoreDbContext context, TestEntity entity)
    {
        var repository = CreateRepository(context);
        var r = await repository.AddAsync(entity);

        r.IsSuccessful.Should().BeTrue();
        context.TestEntities.SingleOrDefault(x => x.Id == entity.Id).Should().NotBeNull();
        r.Data.Should().BeEquivalentTo(context.TestEntities.SingleOrDefault(x => x.Id == entity.Id));
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_AddAsync_Null(TestCoreDbContext context)
    {
        var repository = CreateRepository(context);
        var r = await repository.AddAsync(null!);

        r.IsSuccessful.Should().BeFalse();
        r.Errors.FirstOrDefault().Should().NotBeNull();
        r.Errors.First().Should().Be($"Entity {typeof(TestEntity).FullName} is null");
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_AddAsync_Duplicate(TestCoreDbContext context, TestEntity entity)
    {
        var repository = CreateRepository(context);
        await repository.AddAsync(entity);

        var r = await repository.AddAsync(new TestEntity
        {
            Id = entity.Id
        });

        r.IsSuccessful.Should().BeFalse();
        r.Errors.FirstOrDefault().Should().NotBeNull();
    }
    
    [Theory]
    [DefaultAutoData]
    public void TestRepository_GetSingleById_Success(TestCoreDbContext context, TestEntity entity)
    {
        var repository = CreateRepository(context);
        repository.Add(entity);
    
        repository.GetSingle(entity.Id).Should().Be(entity);
    }
    
    [Theory]
    [DefaultAutoData]
    public void TestRepository_GetSingleById_Null(TestCoreDbContext context, int id)
    {
        var repository = CreateRepository(context);
    
        repository.GetSingle(id).Should().BeNull();
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_GetSingleByIdAsync(TestCoreDbContext context, TestEntity entity)
    {
        var repository = CreateRepository(context);
        await repository.AddAsync(entity);
    
        (await repository.GetSingleAsync(entity.Id)).Should().Be(entity);
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_GetSingleByIdAsync_Null(TestCoreDbContext context, int id)
    {
        var repository = CreateRepository(context);
    
        (await repository.GetSingleAsync(id)).Should().BeNull();
    }
    
    [Theory]
    [DefaultAutoData]
    public void TestRepository_GetSingle(TestCoreDbContext context, TestEntity entity)
    {
        var repository = CreateRepository(context);
        repository.Add(entity);
    
        repository.GetSingle(x => x.Id == entity.Id).Should().Be(entity);
    }
    
    [Theory]
    [DefaultAutoData]
    public void TestRepository_GetSingle_Null(TestCoreDbContext context, int id)
    {
        var repository = CreateRepository(context);
    
        repository.GetSingle(x => x.Id == id).Should().BeNull();
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_GetSingleAsync(TestCoreDbContext context, TestEntity entity)
    {
        var repository = CreateRepository(context);
        await repository.AddAsync(entity);
    
        (await repository.GetSingleAsync(x => x.Id == entity.Id)).Should().Be(entity);
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_GetSingleAsync_Null(TestCoreDbContext context, int id)
    {
        var repository = CreateRepository(context);
    
        (await repository.GetSingleAsync(x => x.Id == id)).Should().BeNull();
    }
    
    [Theory]
    [DefaultAutoData]
    public void TestRepository_Fetch(TestCoreDbContext context, TestEntity entity1, TestEntity entity2)
    {
        var repository = CreateRepository(context);
        repository.Add(entity1);
        repository.Add(entity2);
    
        repository.Fetch().Count().Should().Be(2);
        repository.Fetch().Should().Equal([entity1,entity2]);
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_FetchAsync(TestCoreDbContext context, TestEntity entity1, TestEntity entity2)
    {
        var repository = CreateRepository(context);
        await repository.AddAsync(entity1);
        await repository.AddAsync(entity2);

        (await repository.FetchAsync().CountAsync()).Should().Be(2);
        (await repository.FetchAsync().ToListAsync()).Should().Equal([entity1,entity2]);
    }
    
    [Theory]
    [DefaultAutoData]
    public void TestRepository_Delete_Success(TestCoreDbContext context, TestEntity entity)
    {
        var repository = CreateRepository(context);
        repository.Add(entity);

        var r = repository.Delete(entity);
        
        r.IsSuccessful.Should().BeTrue();
        context.TestEntities.SingleOrDefault(x => x.Id == entity.Id).Should().BeNull();
    }
    
    [Theory]
    [DefaultAutoData]
    public void TestRepository_Delete_NotFound(TestCoreDbContext context, TestEntity entity)
    {
        var repository = CreateRepository(context);

        var r = repository.Delete(entity);
        
        r.IsSuccessful.Should().BeFalse();
    }
    
    [Theory]
    [DefaultAutoData]
    public void TestRepository_Delete_Null(TestCoreDbContext context)
    {
        var repository = CreateRepository(context);

        var r = repository.Delete(null!);
        
        r.IsSuccessful.Should().BeFalse();
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_DeleteAsync_Success(TestCoreDbContext context, TestEntity entity)
    {
        var repository = CreateRepository(context);
        await repository.AddAsync(entity);

        var r = await repository.DeleteAsync(entity);
        
        r.IsSuccessful.Should().BeTrue();
        context.TestEntities.SingleOrDefault(x => x.Id == entity.Id).Should().BeNull();
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_DeleteAsync_NotFound(TestCoreDbContext context, TestEntity entity)
    {
        var repository = CreateRepository(context);

        var r = await repository.DeleteAsync(entity);
        
        r.IsSuccessful.Should().BeFalse();
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_DeleteAsync_Null(TestCoreDbContext context)
    {
        var repository = CreateRepository(context);

        var r = await repository.DeleteAsync(null!);
        
        r.IsSuccessful.Should().BeFalse();
    }
    
    [Theory]
    [DefaultAutoData]
    public void TestRepository_Update_Success(TestCoreDbContext context, TestEntity entity, string randomString)
    {
        var repository = CreateRepository(context);
        repository.Add(entity);

        entity.Value = randomString;

        var r = repository.Update(entity);
        
        r.IsSuccessful.Should().BeTrue();
        context.TestEntities.SingleOrDefault(x => x.Id == entity.Id).Should().NotBeNull();
        r.Data.Should().BeEquivalentTo(context.TestEntities.SingleOrDefault(x => x.Id == entity.Id));
        context.TestEntities.Single(x => x.Id == entity.Id).Value.Should().Be(randomString);
    }
    
    [Theory]
    [DefaultAutoData]
    public void TestRepository_Update_Null(TestCoreDbContext context, TestEntity entity, string randomString)
    {
        var repository = CreateRepository(context);
        repository.Add(entity);

        entity.Value = randomString;

        var r = repository.Update(null!);
        
        r.IsSuccessful.Should().BeFalse();
    }
    
    [Theory]
    [DefaultAutoData]
    public void TestRepository_Update_NotFound(TestCoreDbContext context, TestEntity entity, string randomString)
    {
        var repository = CreateRepository(context);

        entity.Value = randomString;

        var r = repository.Update(null!);
        
        r.IsSuccessful.Should().BeFalse();
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_UpdateAsync_Success(TestCoreDbContext context, TestEntity entity, string randomString)
    {
        var repository = CreateRepository(context);
        await repository.AddAsync(entity);

        entity.Value = randomString;

        var r = await repository.UpdateAsync(entity);
        
        r.IsSuccessful.Should().BeTrue();
        context.TestEntities.SingleOrDefault(x => x.Id == entity.Id).Should().NotBeNull();
        r.Data.Should().BeEquivalentTo(context.TestEntities.SingleOrDefault(x => x.Id == entity.Id));
        context.TestEntities.Single(x => x.Id == entity.Id).Value.Should().Be(randomString);
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_UpdateAsync_Null(TestCoreDbContext context, TestEntity entity, string randomString)
    {
        var repository = CreateRepository(context);
        await repository.AddAsync(entity);

        entity.Value = randomString;

        var r = await repository.UpdateAsync(null!);
        
        r.IsSuccessful.Should().BeFalse();
    }
    
    [Theory]
    [DefaultAutoData]
    public async Task TestRepository_UpdateAsync_NotFound(TestCoreDbContext context, TestEntity entity, string randomString)
    {
        var repository = CreateRepository(context);

        entity.Value = randomString;

        var r = await repository.UpdateAsync(entity);
        
        r.IsSuccessful.Should().BeFalse();
    }

    private static TestRepository CreateRepository(TestCoreDbContext context)
    {
        return new TestRepository(context);
    }
}