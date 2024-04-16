using AutoFixture;
using AutoFixture.Xunit2;
using illyW.Framework.Tests.EFCore.Shared;
using illyW.Framework.Tests.EventStore.Shared;
using illyW.Framework.Tests.EventStore.Shared.Builders;
using illyW.Framework.Tests.Shared.Builders;

namespace illyW.Framework.Tests.Shared.Attributes
{
    public sealed class DefaultAutoDataAttribute() : AutoDataAttribute(Build)
    {
        private static Fixture Build()
        {
            var fixture = new Fixture();

            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            fixture.Customizations.Add(new IntBuilder());

            fixture.Customizations.Add(new DbContextBuilder<TestEventStoreDbContext>());
            fixture.Customizations.Add(new DbContextBuilder<TestCoreDbContext>());
            
            fixture.Customizations.Add(new StreamCheckpointBuilder());
            fixture.Customizations.Add(new AllCheckpointBuilder());
            
            return fixture;
        }
    }
}
