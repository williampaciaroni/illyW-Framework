using System.ComponentModel.DataAnnotations;
using illyW.Framework.Core.GenericEntityPattern;

namespace illyW.Framework.Tests.EFCore.Shared;

public class TestEntity : IEntity<int>
{
    public int Id { get; set; }
    [MaxLength(64)] public string? Value { get; set; }

    public int? TestEntity2Id { get; set; }
    public TestEntity2? TestEntity2 { get; set; }
}

public class TestEntity2 : IEntity<int>
{
    public int Id { get; set; }
    [MaxLength(64)] public string? Value { get; set; }
    public int? TestEntity3Id { get; set; }
    public TestEntity3? TestEntity3 { get; set; }
}

public class TestEntity3 : IEntity<int>
{
    public int Id { get; set; }
    [MaxLength(64)] public string? Value { get; set; }
}