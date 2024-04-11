using AutoFixture.Xunit2;

namespace illyW.Framework.Tests.Shared.Attributes
{
    public sealed class InlineDefaultAutoDataAttribute : InlineAutoDataAttribute
    {
        public InlineDefaultAutoDataAttribute(params object?[] values)
            : base(new DefaultAutoDataAttribute(), values)
        {
        }
    }
}
