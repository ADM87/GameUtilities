using ADM87.GameUtilities.Services;

namespace GameUtilities.Tests
{
    /// <summary>
    /// This class contains unit tests for the PassServiceLocator class.
    /// </summary>
    [TestFixture]
    public class PassServiceLocatorTests
    {
        /// <summary>
        /// Tests the automated collection of service definition.
        /// </summary>
        [Test, Order(1)]
        public static void PassCollectServiceDefinitions()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.CollectServiceDefinitions();
            Assert.Multiple(() =>
            {
                Assert.That(ServiceLocator.Collection,                                                     Is.Not.Empty);
                Assert.That(ServiceLocator.Collection.ContainsKey(typeof(ICollectedService)),              Is.True);
                Assert.That(ServiceLocator.Collection.ContainsKey(typeof(ICollectedSingletonService)),     Is.True);
                Assert.That(ServiceLocator.Collection[typeof(ICollectedService)].ServiceLifeTime,          Is.EqualTo(EServiceLifeTime.Transient));
                Assert.That(ServiceLocator.Collection[typeof(ICollectedSingletonService)].ServiceLifeTime, Is.EqualTo(EServiceLifeTime.Singleton));
            });
        }

        /// <summary>
        /// Tests the manual addition of service definitions to the service locator.
        /// </summary>
        [Test, Order(2)]
        public static void PassManualAddServiceDefinition()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IManualService, ManualService>();
            ServiceLocator.AddService<IManualSingletonService, ManualSingletonService>(EServiceLifeTime.Singleton);
            Assert.Multiple(() =>
            {
                Assert.That(ServiceLocator.Collection,                                                     Is.Not.Empty);
                Assert.That(ServiceLocator.Collection.ContainsKey(typeof(IManualService)),                 Is.True);
                Assert.That(ServiceLocator.Collection.ContainsKey(typeof(IManualSingletonService)),        Is.True);
                Assert.That(ServiceLocator.Collection[typeof(IManualService)].ServiceLifeTime,             Is.EqualTo(EServiceLifeTime.Transient));
                Assert.That(ServiceLocator.Collection[typeof(IManualSingletonService)].ServiceLifeTime,    Is.EqualTo(EServiceLifeTime.Singleton));
            });
        }

        /// <summary>
        /// Tests accessing non-singleton services from the service locator.
        /// </summary>
        [Test, Order(3)]
        public static void PassGetServiceInstance()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IManualService, ManualService>();
            Assert.Multiple(() =>
            {
                IManualService serviceA = ServiceLocator.GetService<IManualService>();
                Assert.That(serviceA, Is.Not.Null);
                Assert.That(serviceA, Is.TypeOf<ManualService>());

                IManualService serviceB = ServiceLocator.GetService<IManualService>();
                Assert.That(serviceA, Is.Not.EqualTo(serviceB));
            });
        }

        /// <summary>
        /// Test case for retrieving a singleton service instance from the service locator.
        /// </summary>
        [Test, Order(4)]
        public static void PassGetSingletonServiceInstance()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IManualSingletonService, ManualSingletonService>(EServiceLifeTime.Singleton);
            Assert.Multiple(() =>
            {
                IManualSingletonService serviceA = ServiceLocator.GetService<IManualSingletonService>();
                Assert.That(serviceA, Is.Not.Null);
                Assert.That(serviceA, Is.TypeOf<ManualSingletonService>());

                IManualSingletonService serviceB = ServiceLocator.GetService<IManualSingletonService>();
                Assert.That(serviceA, Is.EqualTo(serviceB));
            });
        }

        /// <summary>
        /// Tests service dependencies are resolved correctly.
        /// </summary>
        [Test, Order(5)]
        public static void PassGetServiceWithDependency()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IManualService, ManualService>();
            ServiceLocator.AddService<IManualServiceWithDependency, ManualServiceWithDependency>();
            Assert.Multiple(() =>
            {
                IManualServiceWithDependency service = ServiceLocator.GetService<IManualServiceWithDependency>();
                Assert.That(service.ASimpleManualService, Is.TypeOf<ManualService>());
            });
        }

        /// <summary>
        /// Tests the retrieval of a service with a singleton dependency from the service locator.
        /// </summary>
        [Test, Order(6)]
        public static void PassGetServiceWithSingletonDependency()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IManualSingletonService, ManualSingletonService>(EServiceLifeTime.Singleton);
            ServiceLocator.AddService<IManualServiceWithSingletonDependency, ManualServiceWithSingletonDependency>();
            Assert.Multiple(() =>
            {
                IManualServiceWithSingletonDependency serviceA = ServiceLocator.GetService<IManualServiceWithSingletonDependency>();
                Assert.That(serviceA.ASingletonManualService, Is.TypeOf<ManualSingletonService>());

                IManualServiceWithSingletonDependency serviceB = ServiceLocator.GetService<IManualServiceWithSingletonDependency>();
                Assert.That(serviceA.ASingletonManualService, Is.EqualTo(serviceB.ASingletonManualService));
            });
        }

        [Test, Order(7)]
        public static void PassServiceDependencyInjection()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<ICollectedService, CollectedService>();
            InjectionTarget target = new InjectionTarget();
            ServiceLocator.ResolveDependencies(target);
            Assert.Multiple(() =>
            {
                Assert.That(target.CollectedService, Is.Not.Null);
                Assert.That(target.CollectedService, Is.TypeOf<CollectedService>());
            });
        }
    }
}
