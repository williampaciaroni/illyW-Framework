using System.Collections.ObjectModel;
using System.Reflection;
using AutoFixture.Kernel;
using illyW.Framework.Tests.Shared.Attributes;
using CollectionAttribute = illyW.Framework.Tests.Shared.Attributes.CollectionAttribute;

namespace illyW.Framework.Tests.Shared
{
    public class IntBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var customAttributeProvider = request as ICustomAttributeProvider;
            if (customAttributeProvider == null)
            {
                return new NoSpecimen();
            }

            var intAttribute =
                customAttributeProvider.GetCustomAttributes(typeof(IntAttribute), true)
                    .OfType<IntAttribute>()
                    .FirstOrDefault();

            if (intAttribute == null)
            {
                return new NoSpecimen();
            }

            var collectionAttribute =
                customAttributeProvider.GetCustomAttributes(typeof(CollectionAttribute), true)
                    .OfType<CollectionAttribute>()
                    .FirstOrDefault();

            if (collectionAttribute != null)
            {
                var collection = new Collection<int>();
                for (var i = 0; i < collectionAttribute.Count; i++)
                {
                    collection.Add(CreateInt(intAttribute));
                }

                return collection;
            }

            return CreateInt(intAttribute);
        }

        private static int CreateInt(IntAttribute intAttribute)
        {
            var rand = new Random();
#pragma warning disable CA5394
            return rand.Next(intAttribute.Min, intAttribute.Max);
#pragma warning restore CA5394
        }
    }
}
