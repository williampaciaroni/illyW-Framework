using illyW.Framework.EFCore;

namespace illyW.Framework.Tests.EFCore.Shared;

public class TestRepository(TestCoreDbContext context)
    : GenericRepository<TestEntity, int, TestCoreDbContext>(context), ITestRepository;