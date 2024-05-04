using ADM87.GameUtilities.Services;

namespace GameUtilities.Tests
{
    /// <summary>
    /// Contains test methods to verify the failure scenarios of the service provider.
    /// </summary>
    [TestFixture]
    public static class FailServiceProviderTests
    {
        /// <summary>
        /// Test method to verify the failure scenario when an invalid service identity type is used.
        /// </summary>
        [Test]
        public static void FailInvalidServiceIdentitiy()
        {
            ServiceProvider.Collection.Clear();
            Assert.Throws<InvalidServiceIdentityTypeException>(() => {
                ServiceProvider.AddService<IInvalidServiceIdentityType, InvalidServiceIdentityType>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when an invalid service implementation type is used.
        /// </summary>
        [Test]
        public static void FailInvalidServiceImplementation()
        {
            ServiceProvider.Collection.Clear();
            Assert.Throws<InvalidServiceImplementationException>(() => {
                ServiceProvider.AddService<IInvalidServiceImplementationType, InvalidServiceImplementationType>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when a duplicate service identity is added.
        /// </summary>
        [Test]
        public static void FailDuplicateServiceIdentity()
        {
            ServiceProvider.Collection.Clear();
            ServiceProvider.AddService<IDuplicateIdentityType, DuplicateIdentityTypeA>();
            Assert.Throws<DuplicateServiceIdentityException>(() => {
                ServiceProvider.AddService<IDuplicateIdentityType, DuplicateIdentitiyTypeB>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when a duplicate service implementation is added.
        /// </summary>
        [Test]
        public static void FailDuplicateServiceImplementation()
        {
            ServiceProvider.Collection.Clear();
            ServiceProvider.AddService<IDuplicateImplementationTypeA, DuplicateImplementationType>();
            Assert.Throws<DuplicateServiceImplementationException>(() => {
                ServiceProvider.AddService<IDuplicateImplementationTypeB, DuplicateImplementationType>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when a service dependency private setter is missing.
        /// </summary>
        [Test]
        public static void FailMissingPrivateDependencySetter()
        {
            ServiceProvider.Collection.Clear();
            Assert.Throws<MissingServiceDependencySetterException>(() => {
                ServiceProvider.AddService<IMissingPrivateDependencySetter, MissingPrivateDependencySetter>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when an invalid service dependency type is used.
        /// </summary>
        [Test]
        public static void FailInvalidServiceDependencyType()
        {
            ServiceProvider.Collection.Clear();
            Assert.Throws<InvalidServiceDependencyTypeException>(() => {
                ServiceProvider.AddService<IInvalidServiceDependencyType, InvalidServiceDependencyType>();
            });
        }

        /// <summary>
        /// Tests the scenario where a service is not found in the service collection.
        /// </summary>
        [Test]
        public static void FailServiceNotFound()
        {
            ServiceProvider.Collection.Clear();
            Assert.Throws<ServiceNotFoundException>(() => ServiceProvider.Get<ITestService>());
        }

        /// <summary>
        /// Tests the scenario where there is a circular dependency between services.
        /// </summary>
        [Test]
        public static void FailCircularDependency()
        {
            ServiceProvider.Collection.Clear();
            ServiceProvider.AddService<ICircularDependencyA, CircularDependencyA>();
            Assert.Multiple(() => {
                Assert.Throws<CircularServiceDependencyException>(() => {
                    ServiceProvider.AddService<ICircularDependencyB, CircularDependencyB>();
                });
                Assert.Throws<CircularServiceDependencyException>(() => {
                    ServiceProvider.AddService<ICircularDependencyC, CircularDependencyC>();
                });
                Assert.Throws<CircularServiceDependencyException>(() => {
                    ServiceProvider.AddService<ICircularDependencyD, CircularDependencyD>();
                });
                Assert.Throws<CircularServiceDependencyException>(() => {
                    ServiceProvider.AddService<ICircularDependencyE, CircularDependencyE>();
                });
            });
        }
    }
}
