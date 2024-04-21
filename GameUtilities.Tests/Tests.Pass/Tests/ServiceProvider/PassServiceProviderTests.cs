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
        [Test, Order(1)]
        public static void PassCollectServiceDefinitions()
        {
            Services.Collection.Clear();
            Services.CollectServiceDefinitions();
            Assert.Multiple(() =>
            {
                Assert.That(Services.Collection,                                                        Is.Not.Empty);
                Assert.That(Services.Collection.ContainsKey(typeof(ICollectedService)),                 Is.True);
                Assert.That(Services.Collection.ContainsKey(typeof(ICollectedSingletonService)),        Is.True);
                Assert.That(Services.Collection[typeof(ICollectedService)].IsSingleton,                 Is.False);
                Assert.That(Services.Collection[typeof(ICollectedSingletonService)].IsSingleton,        Is.True);
            });
        }

        /// <summary>
        /// Tests the manual addition of service definitions to the service provider.
        /// </summary>
        [Test, Order(2)]
        public static void PassManualAddServiceDefinition()
        {
            Services.Collection.Clear();
            Services.AddService<IManualService, ManualService>();
            Services.AddService<IManualSingletonService, ManualSingletonService>(isSingleton: true);
            Assert.Multiple(() =>
            {
                Assert.That(Services.Collection,                                                    Is.Not.Empty);
                Assert.That(Services.Collection.ContainsKey(typeof(IManualService)),                Is.True);
                Assert.That(Services.Collection.ContainsKey(typeof(IManualSingletonService)),       Is.True);
                Assert.That(Services.Collection[typeof(IManualService)].IsSingleton,                Is.False);
                Assert.That(Services.Collection[typeof(IManualSingletonService)].IsSingleton,       Is.True);
            });
        }

        /// <summary>
        /// Tests accessing non-singleton services from the service provider.
        /// </summary>
        [Test, Order(3)]
        public static void PassGetServiceInstance()
        {
            Services.Collection.Clear();
            Services.AddService<IManualService, ManualService>();
            Assert.Multiple(() =>
            {
                IManualService serviceA = Services.Get<IManualService>();
                Assert.That(serviceA, Is.Not.Null);
                Assert.That(serviceA, Is.TypeOf<ManualService>());

                IManualService serviceB = Services.Get<IManualService>();
                Assert.That(serviceA, Is.Not.EqualTo(serviceB));
            });
        }

        /// <summary>
        /// Test case for retrieving a singleton service instance from the service provider.
        /// </summary>
        [Test, Order(4)]
        public static void PassGetSingletonServiceInstance()
        {
            Services.Collection.Clear();
            Services.AddService<IManualSingletonService, ManualSingletonService>(isSingleton: true);
            Assert.Multiple(() =>
            {
                IManualSingletonService serviceA = Services.Get<IManualSingletonService>();
                Assert.That(serviceA, Is.Not.Null);
                Assert.That(serviceA, Is.TypeOf<ManualSingletonService>());

                IManualSingletonService serviceB = Services.Get<IManualSingletonService>();
                Assert.That(serviceA, Is.EqualTo(serviceB));
            });
        }

        /// <summary>
        /// Tests service dependencies are resolved correctly.
        /// </summary>
        [Test, Order(5)]
        public static void PassGetServiceWithDependency()
        {
            Services.Collection.Clear();
            Services.AddService<IManualService, ManualService>();
            Services.AddService<IManualServiceWithDependency, ManualServiceWithDependency>();
            Assert.Multiple(() =>
            {
                IManualServiceWithDependency service = Services.Get<IManualServiceWithDependency>();
                Assert.That(service.ASimpleManualService, Is.TypeOf<ManualService>());
            });
        }

        /// <summary>
        /// Tests the retrieval of a service with a singleton dependency from the service provider.
        /// </summary>
        [Test, Order(6)]
        public static void PassGetServiceWithSingletonDependency()
        {
            Services.Collection.Clear();
            Services.AddService<IManualSingletonService, ManualSingletonService>(isSingleton: true);
            Services.AddService<IManualServiceWithSingletonDependency, ManualServiceWithSingletonDependency>();
            Assert.Multiple(() =>
            {
                IManualServiceWithSingletonDependency serviceA = Services.Get<IManualServiceWithSingletonDependency>();
                Assert.That(serviceA.ASingletonManualService, Is.TypeOf<ManualSingletonService>());

                IManualServiceWithSingletonDependency serviceB = Services.Get<IManualServiceWithSingletonDependency>();
                Assert.That(serviceA.ASingletonManualService, Is.EqualTo(serviceB.ASingletonManualService));
            });
        }
    }
}