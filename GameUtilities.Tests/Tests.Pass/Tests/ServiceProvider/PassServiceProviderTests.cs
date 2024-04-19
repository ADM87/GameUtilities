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
                    Assert.That(Services.Collection.ContainsKey(typeof(ICollectedService)),                 Is.True);
                    Assert.That(Services.Collection.ContainsKey(typeof(ICollectedServiceWithDependency)),   Is.True);
                    Assert.That(Services.Collection.ContainsKey(typeof(ICollectedSingletonService)),        Is.True);
                    Assert.That(Services.Collection[typeof(ICollectedService)].IsSingleton,                 Is.False);
                    Assert.That(Services.Collection[typeof(ICollectedServiceWithDependency)].IsSingleton,   Is.False);
                    Assert.That(Services.Collection[typeof(ICollectedSingletonService)].IsSingleton,        Is.True);
                    // TODO - Check for service dependency injection on CollectedServiceWithDependency
                });
            }

            /// <summary>
            /// Tests the manual addition of service definitions to the service provider.
            /// </summary>
            [Test]
            public static void PassManualAddServiceDefinition()
            {
                Services.Collection.Clear();
                Services.AddService<IManualService, ManualService>();
                Services.AddService<IManualServiceWithDependency, ManualServiceWithDependency>();
                Services.AddService<IManualSingletonService, ManualSingletonService>(isSingleton: true);
                Assert.Multiple(() =>
                {
                    Assert.That(Services.Collection,                                                    Is.Not.Empty);
                    Assert.That(Services.Collection.ContainsKey(typeof(IManualService)),                Is.True);
                    Assert.That(Services.Collection.ContainsKey(typeof(IManualServiceWithDependency)),  Is.True);
                    Assert.That(Services.Collection.ContainsKey(typeof(IManualSingletonService)),       Is.True);
                    Assert.That(Services.Collection[typeof(IManualService)].IsSingleton,                Is.False);
                    Assert.That(Services.Collection[typeof(IManualServiceWithDependency)].IsSingleton,  Is.False);
                    Assert.That(Services.Collection[typeof(IManualSingletonService)].IsSingleton,       Is.True);
                    // TODO - Check for service dependency injection on ManualServiceWithDependency
                });
            }
        }
}
