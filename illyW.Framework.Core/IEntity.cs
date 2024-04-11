using System;

namespace illyW.Framework.Core
{
    public interface IEntity<T> 
        where T : IComparable, IEquatable<T>
    {
        T Id { get; set; }
    }
}
