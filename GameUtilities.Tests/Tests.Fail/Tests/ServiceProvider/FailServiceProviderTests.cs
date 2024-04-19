using ADM87.GameUtilities.ServiceProvider;

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
            Services.Collection.Clear();
            Assert.Throws<InvalidServiceIdentityTypeException>(() => {
                Services.AddService<IInvalidServiceIdentityType, InvalidServiceIdentityType>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when an invalid service implementation type is used.
        /// </summary>
        [Test]
        public static void FailInvalidServiceImplementation()
        {
            Services.Collection.Clear();
            Assert.Throws<InvalidServiceImplementationException>(() => {
                Services.AddService<IInvalidServiceImplementationType, InvalidServiceImplementationType>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when a duplicate service identity is added.
        /// </summary>
        [Test]
        public static void FailDuplicateServiceIdentity()
        {
            Services.Collection.Clear();
            Services.AddService<IDuplicateIdentityType, DuplicateIdentityTypeA>();
            Assert.Throws<DuplicateServiceIdentityException>(() => {
                Services.AddService<IDuplicateIdentityType, DuplicateIdentitiyTypeB>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when a duplicate service implementation is added.
        /// </summary>
        [Test]
        public static void FailDuplicateServiceImplementation()
        {
            Services.Collection.Clear();
            Services.AddService<IDuplicateImplementationTypeA, DuplicateImplementationType>();
            Assert.Throws<DuplicateServiceImplementationException>(() => {
                Services.AddService<IDuplicateImplementationTypeB, DuplicateImplementationType>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when a service dependency private setter is missing.
        /// </summary>
        [Test]
        public static void FailMissingPrivateDependencySetter()
        {
            Services.Collection.Clear();
            Assert.Throws<MissingServiceDependencySetterException>(() => {
                Services.AddService<IMissingPrivateDependencySetter, MissingPrivateDependencySetter>();
            });
        }

        /// <summary>
        /// Test method to verify the failure scenario when an invalid service dependency type is used.
        /// </summary>
        [Test]
        public static void FailInvalidServiceDependencyType()
        {
            Services.Collection.Clear();
            Assert.Throws<InvalidServiceDependencyTypeException>(() => {
                Services.AddService<IInvalidServiceDependencyType, InvalidServiceDependencyType>();
            });
        }
    }
}
