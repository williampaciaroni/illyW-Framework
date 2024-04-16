using System;

namespace illyW.Framework.Core.GenericEntityPattern
{
    public interface IEntity<T> 
        where T : IComparable, IEquatable<T>
    {
        T Id { get; set; }
    }
}
