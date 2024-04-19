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
                    Assert.That(Services.Collection,                                                    Is.Not.Empty);
                    Assert.That(Services.Collection[typeof(ISimpleCollectedService)].IsSingleton,       Is.False);
                    Assert.That(Services.Collection[typeof(ICollectedSingletonService)].IsSingleton,    Is.True);
                });
            }

            /// <summary>
            /// Tests the manual addition of service definitions to the service provider.
            /// </summary>
            [Test]
            public static void PassManualAddServiceDefinition()
            {
                Services.Collection.Clear();
                Services.AddService<ISimpleManualService, SimpleManualService>();
                Services.AddService<IManualServiceWidthDependency, ManualServiceWithDependency>();
                Services.AddService<ICollectedSingletonService, CollectedSingletonService>(isSingleton: true);
                Assert.Multiple(() =>
                {
                    Assert.That(Services.Collection,                                                    Is.Not.Empty);
                    Assert.That(Services.Collection[typeof(ISimpleManualService)].IsSingleton,          Is.False);
                    Assert.That(Services.Collection[typeof(IManualServiceWidthDependency)].IsSingleton, Is.False);
                    // TODO - Check for service dependency injection on ManualServiceWithDependency
                    Assert.That(Services.Collection[typeof(ICollectedSingletonService)].IsSingleton,    Is.True);
                });
            }
        }
}
