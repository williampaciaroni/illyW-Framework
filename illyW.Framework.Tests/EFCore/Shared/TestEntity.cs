using illyW.Framework.Core.GenericEntityPattern;

namespace illyW.Framework.Tests.EFCore.Shared;

public class TestEntity : IEntity<int>
{
    public int Id { get; set; }
    public string Value { get; set; }
}