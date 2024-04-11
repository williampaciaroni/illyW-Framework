using System;

namespace illyW.Framework.Tests.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class CollectionAttribute : Attribute
    {
        public CollectionAttribute(int count)
        {
            Count = count;
        }

        public int Count { get; }
    }
}
