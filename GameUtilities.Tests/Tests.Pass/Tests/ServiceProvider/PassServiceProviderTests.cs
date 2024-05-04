using ADM87.GameUtilities.Services;

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
            ServiceProvider.Collection.Clear();
            ServiceProvider.CollectServiceDefinitions();
            Assert.Multiple(() =>
            {
                Assert.That(ServiceProvider.Collection,                                                        Is.Not.Empty);
                Assert.That(ServiceProvider.Collection.ContainsKey(typeof(ICollectedService)),                 Is.True);
                Assert.That(ServiceProvider.Collection.ContainsKey(typeof(ICollectedSingletonService)),        Is.True);
                Assert.That(ServiceProvider.Collection[typeof(ICollectedService)].IsSingleton,                 Is.False);
                Assert.That(ServiceProvider.Collection[typeof(ICollectedSingletonService)].IsSingleton,        Is.True);
            });
        }

        /// <summary>
        /// Tests the manual addition of service definitions to the service provider.
        /// </summary>
        [Test, Order(2)]
        public static void PassManualAddServiceDefinition()
        {
            ServiceProvider.Collection.Clear();
            ServiceProvider.AddService<IManualService, ManualService>();
            ServiceProvider.AddService<IManualSingletonService, ManualSingletonService>(isSingleton: true);
            Assert.Multiple(() =>
            {
                Assert.That(ServiceProvider.Collection,                                                    Is.Not.Empty);
                Assert.That(ServiceProvider.Collection.ContainsKey(typeof(IManualService)),                Is.True);
                Assert.That(ServiceProvider.Collection.ContainsKey(typeof(IManualSingletonService)),       Is.True);
                Assert.That(ServiceProvider.Collection[typeof(IManualService)].IsSingleton,                Is.False);
                Assert.That(ServiceProvider.Collection[typeof(IManualSingletonService)].IsSingleton,       Is.True);
            });
        }

        /// <summary>
        /// Tests accessing non-singleton services from the service provider.
        /// </summary>
        [Test, Order(3)]
        public static void PassGetServiceInstance()
        {
            ServiceProvider.Collection.Clear();
            ServiceProvider.AddService<IManualService, ManualService>();
            Assert.Multiple(() =>
            {
                IManualService serviceA = ServiceProvider.Get<IManualService>();
                Assert.That(serviceA, Is.Not.Null);
                Assert.That(serviceA, Is.TypeOf<ManualService>());

                IManualService serviceB = ServiceProvider.Get<IManualService>();
                Assert.That(serviceA, Is.Not.EqualTo(serviceB));
            });
        }

        /// <summary>
        /// Test case for retrieving a singleton service instance from the service provider.
        /// </summary>
        [Test, Order(4)]
        public static void PassGetSingletonServiceInstance()
        {
            ServiceProvider.Collection.Clear();
            ServiceProvider.AddService<IManualSingletonService, ManualSingletonService>(isSingleton: true);
            Assert.Multiple(() =>
            {
                IManualSingletonService serviceA = ServiceProvider.Get<IManualSingletonService>();
                Assert.That(serviceA, Is.Not.Null);
                Assert.That(serviceA, Is.TypeOf<ManualSingletonService>());

                IManualSingletonService serviceB = ServiceProvider.Get<IManualSingletonService>();
                Assert.That(serviceA, Is.EqualTo(serviceB));
            });
        }

        /// <summary>
        /// Tests service dependencies are resolved correctly.
        /// </summary>
        [Test, Order(5)]
        public static void PassGetServiceWithDependency()
        {
            ServiceProvider.Collection.Clear();
            ServiceProvider.AddService<IManualService, ManualService>();
            ServiceProvider.AddService<IManualServiceWithDependency, ManualServiceWithDependency>();
            Assert.Multiple(() =>
            {
                IManualServiceWithDependency service = ServiceProvider.Get<IManualServiceWithDependency>();
                Assert.That(service.ASimpleManualService, Is.TypeOf<ManualService>());
            });
        }

        /// <summary>
        /// Tests the retrieval of a service with a singleton dependency from the service provider.
        /// </summary>
        [Test, Order(6)]
        public static void PassGetServiceWithSingletonDependency()
        {
            ServiceProvider.Collection.Clear();
            ServiceProvider.AddService<IManualSingletonService, ManualSingletonService>(isSingleton: true);
            ServiceProvider.AddService<IManualServiceWithSingletonDependency, ManualServiceWithSingletonDependency>();
            Assert.Multiple(() =>
            {
                IManualServiceWithSingletonDependency serviceA = ServiceProvider.Get<IManualServiceWithSingletonDependency>();
                Assert.That(serviceA.ASingletonManualService, Is.TypeOf<ManualSingletonService>());

                IManualServiceWithSingletonDependency serviceB = ServiceProvider.Get<IManualServiceWithSingletonDependency>();
                Assert.That(serviceA.ASingletonManualService, Is.EqualTo(serviceB.ASingletonManualService));
            });
        }

        [Test, Order(7)]
        public static void PassServiceDependencyInjection()
        {
            ServiceProvider.Collection.Clear();
            ServiceProvider.AddService<ICollectedService, CollectedService>();
            InjectionTarget target = new InjectionTarget();
            ServiceProvider.InjectDependencies(target);
            Assert.Multiple(() =>
            {
                Assert.That(target.CollectedService, Is.Not.Null);
                Assert.That(target.CollectedService, Is.TypeOf<CollectedService>());
            });
        }
    }
}
