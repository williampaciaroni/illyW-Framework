![NuGet Version](https://img.shields.io/nuget/v/illyW.Framework.Core?style=plastic&label=illyW.Framework.Core&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FillyW.Framework.Core)
![NuGet Version](https://img.shields.io/nuget/v/illyW.Framework.EFCore?style=plastic&label=illyW.Framework.EFCore&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FillyW.Framework.EFCore)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=williampaciaroni_illyW-Framework&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=williampaciaroni_illyW-Framework)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=williampaciaroni_illyW-Framework&metric=coverage)](https://sonarcloud.io/summary/new_code?id=williampaciaroni_illyW-Framework)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=williampaciaroni_illyW-Framework&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=williampaciaroni_illyW-Framework)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=williampaciaroni_illyW-Framework&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=williampaciaroni_illyW-Framework)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=williampaciaroni_illyW-Framework&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=williampaciaroni_illyW-Framework)

# illyW-Framework

**illyW-Framework** is a .NET framework that provides a set of utilities to facilitate application development by implementing some of the most common and powerful patterns like:

- **Generic Entity**
- **Repository**
- **Unit of Work**
- **Result**

This framework aims to reduce boilerplate code by offering generic implementations that can be easily extended or integrated into existing projects.

## Installation

You can install **illyW-Framework** via NuGet. Run the following command in the Package Manager Console:

```bash
Install-Package illyW.Framework.Core
Install-Package illyW.Framework.EFCore
```
## Usage

### Generic Entity Pattern
The IEntity interface is a base interface which can be implemented to create entities with common identifiers.

```cs
public class TestEntity : IEntity<int>
{
    public int Id { get;set; }
    public string Name { get; set; }
}
```
```cs
public class TestEntity2 : IEntity<string>
{
    public string Id { get;set; }
    public string Name { get; set; }
}
```

### Repository Pattern
The IGenericRepository interface provides a set of methods to support CRUD operations.

```cs
public interface ITestRepository : IGenericRepository<TestEntity, int>;
```

```cs
public class TestCoreDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<TestEntity> TestEntities { get; set; } = null!;
}
```
```cs
public class TestRepository(TestCoreDbContext context)
    : GenericRepository<TestEntity, int, TestCoreDbContext>(context), ITestRepository;
```
```cs
serviceCollection.AddRepository<ITestRepository, TestRepository>();
```

```cs
[ApiController]
[Route("api/[controller]")]
public class TestController(ITestRepository repository) : ControllerBase
{

  [HttpGet]
  public ActionResult GetAll()
  {
      return Ok(repository.Fetch());
  }

}
```

### Unit of Work
The IUnitOfWork interface provides a set of methods to centralize the management of transactional operations, ensuring that changes are applied atomically.

```cs
serviceCollection.AddUnitOfWork<TestCoreDbContext>();
```

```cs
[ApiController]
[Route("api/[controller]")]
public class TestController(IUnitOfWork uow) : ControllerBase
{

  [HttpPost]
  public ActionResult Create([FromBody] string name)
  {
      var repository = uow.GetRepository<ITestRepository>();
      var repository2 = uow.GetRepository<ITest2Repository>();

      var r1 = await repository.AddAsync(new TestEntity {Name = name});
      var r2 = await repository2.AddAsync(new TestEntity2 {Name = name});

      if(r1.IsSuccessful && r2.IsSuccesful)
      {
        uow.Commit();
        return Ok();
      }

      return UnprocessableEntity();
  }

}
```

### Result
The IResult is an interface to represent the outcome of an operation, enabling elegant handling of success, failure, and status messages.


