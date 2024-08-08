using System.ComponentModel.DataAnnotations;
using illyW.Framework.Core.GenericEntityPattern;

namespace illyW.Framework.Tests.EFCore.Shared;

public class TestEntity : IEntity<int>
{
    public int Id { get; set; }
    [MaxLength(64)]
    public string? Value { get; set; }
}