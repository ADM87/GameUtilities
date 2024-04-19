using ADM87.GameUtilities.ServiceProvider;

namespace GameUtilities.Tests
{
        /// <summary>
        /// This class contains unit tests for the PassServiceProvider class.
        /// </summary>
        [TestFixture]
        public class PassServiceProviderTests
        {
            /// <summary>
            /// Tests the automated collection of service definition.
            /// </summary>
            [Test]
            public static void PassCollectServiceDefinitions()
            {
                Services.Collection.Clear();
                Services.CollectServiceDefinitions();
                Assert.Multiple(() =>
                {
                    Assert.That(Services.Collection,                                                        Is.Not.Empty);
                    Assert.That(Services.Collection[typeof(ISuccessCollectService)].IsSingleton,            Is.False);
                    Assert.That(Services.Collection[typeof(ISuccessCollectSingletonService)].IsSingleton,   Is.True);
                });
            }

            /// <summary>
            /// Tests the manual addition of service definitions to the service provider.
            /// </summary>
            [Test]
            public static void PassManualAddServiceDefinition()
            {
                Services.Collection.Clear();
                Services.AddService<ISimpleSuccessService, SimpleSuccessService>();
                Services.AddService<ISuccessServiceWithDependency, SuccessServiceWithDependency>();
                Services.AddService<ISuccessCollectSingletonService, SuccessCollectSingletonService>(isSingleton: true);
                Assert.Multiple(() =>
                {
                    Assert.That(Services.Collection,                                                                Is.Not.Empty);
                    Assert.That(Services.Collection[typeof(ISimpleSuccessService)].IsSingleton,                     Is.False);
                    Assert.That(Services.Collection[typeof(ISuccessCollectSingletonService)].IsSingleton,           Is.True);
                    Assert.That(Services.Collection[typeof(ISuccessServiceWithDependency)].Dependencies.Count(),    Is.GreaterThan(0));
                });
            }
        }
}
