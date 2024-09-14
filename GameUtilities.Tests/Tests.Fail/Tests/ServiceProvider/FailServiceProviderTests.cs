using ADM87.GameUtilities.Services;

namespace GameUtilities.Tests
{
    /// <summary>
    /// Contains test methods to verify the failure scenarios of the service locator.
    /// </summary>
    [TestFixture]
    public static class FailServiceLocatorTests
    {
        /// <summary>
        /// Test method to verify the failure scenario when an invalid service identity type is used.
        /// </summary>
        [Test]
        public static void FailInvalidServiceIdentitiy()
        {
            ServiceLocator.Collection.Clear();
            Assert.Throws<InvalidServiceIdentityTypeException>(() => {
                ServiceLocator.AddService<IInvalidServiceIdentityType, InvalidServiceIdentityType>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when an invalid service implementation type is used.
        /// </summary>
        [Test]
        public static void FailInvalidServiceImplementation()
        {
            ServiceLocator.Collection.Clear();
            Assert.Throws<InvalidServiceImplementationException>(() => {
                ServiceLocator.AddService<IInvalidServiceImplementationType, InvalidServiceImplementationType>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when a duplicate service identity is added.
        /// </summary>
        [Test]
        public static void FailDuplicateServiceIdentity()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IDuplicateIdentityType, DuplicateIdentityTypeA>();
            Assert.Throws<DuplicateServiceIdentityException>(() => {
                ServiceLocator.AddService<IDuplicateIdentityType, DuplicateIdentitiyTypeB>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when a duplicate service implementation is added.
        /// </summary>
        [Test]
        public static void FailDuplicateServiceImplementation()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IDuplicateImplementationTypeA, DuplicateImplementationType>();
            Assert.Throws<DuplicateServiceImplementationException>(() => {
                ServiceLocator.AddService<IDuplicateImplementationTypeB, DuplicateImplementationType>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when a service dependency private setter is missing.
        /// </summary>
        [Test]
        public static void FailMissingPrivateDependencySetter()
        {
            ServiceLocator.Collection.Clear();
            Assert.Throws<MissingServiceDependencySetterException>(() => {
                ServiceLocator.AddService<IMissingPrivateDependencySetter, MissingPrivateDependencySetter>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when an invalid service dependency type is used.
        /// </summary>
        [Test]
        public static void FailInvalidServiceDependencyType()
        {
            ServiceLocator.Collection.Clear();
            Assert.Throws<InvalidServiceDependencyTypeException>(() => {
                ServiceLocator.AddService<IInvalidServiceDependencyType, InvalidServiceDependencyType>();
            });
        }

        /// <summary>
        /// Tests the scenario where a service is not found in the service collection.
        /// </summary>
        [Test]
        public static void FailServiceNotFound()
        {
            ServiceLocator.Collection.Clear();
            Assert.Throws<ServiceNotFoundException>(() => ServiceLocator.GetService<ITestService>());
        }

        /// <summary>
        /// Tests the scenario where there is a circular dependency between services.
        /// </summary>
        [Test]
        public static void FailCircularDependency()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<ICircularDependencyA, CircularDependencyA>();
            Assert.Multiple(() => {
                Assert.Throws<CircularServiceDependencyException>(() => {
                    ServiceLocator.AddService<ICircularDependencyB, CircularDependencyB>();
                });
                Assert.Throws<CircularServiceDependencyException>(() => {
                    ServiceLocator.AddService<ICircularDependencyC, CircularDependencyC>();
                });
                Assert.Throws<CircularServiceDependencyException>(() => {
                    ServiceLocator.AddService<ICircularDependencyD, CircularDependencyD>();
                });
                Assert.Throws<CircularServiceDependencyException>(() => {
                    ServiceLocator.AddService<ICircularDependencyE, CircularDependencyE>();
                });
            });
        }
    }
}
