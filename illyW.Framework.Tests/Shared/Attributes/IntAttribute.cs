using System;

namespace illyW.Framework.Tests.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class IntAttribute : Attribute
    {
        public int Min { get; set; } = int.MinValue;

        public int Max { get; set; } = int.MaxValue;
    }
}
