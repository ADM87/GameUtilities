using ADM87.GameUtilities.ServiceProvider;

namespace GameUtilities.Tests
{
    // Generic service interface and implementation for testing.
    public interface ITestService {}
    public class TestService : ITestService {}

    // Fails because the identity type is not an interface.
    public class IInvalidServiceIdentityType {}
    public class InvalidServiceIdentityType : IInvalidServiceIdentityType {}

    // Fails because the implementation type does not implement the identity type.
    public interface IInvalidServiceImplementationType {}
    public class InvalidServiceImplementationType {}

    // Fails because a service with the same identity type already exists in the service collection.
    public interface IDuplicateIdentityType {}
    public class DuplicateIdentityTypeA : IDuplicateIdentityType {}
    public class DuplicateIdentitiyTypeB : IDuplicateIdentityType {}

    // Fails because a service with the same implementation type already exists in the service collection.
    public interface IDuplicateImplementationTypeA {}
    public interface IDuplicateImplementationTypeB {}
    public class DuplicateImplementationType : IDuplicateImplementationTypeA, IDuplicateImplementationTypeB {}

    // Fails because the implementation's service dependency is circular.
    public interface ICircularDependencyA {}
    public interface ICircularDependencyB {}
    public interface ICircularDependencyC {}
    public interface ICircularDependencyD {}
    public interface ICircularDependencyE {}
    public class CircularDependencyA : ICircularDependencyA
    {
        [ServiceDependency]
        public ICircularDependencyB DependencyB { get; private set; }
    }
    public class CircularDependencyB : ICircularDependencyB
    {
        [ServiceDependency]
        public ICircularDependencyA DependencyA { get; private set; }
    }
    public class CircularDependencyC : ICircularDependencyC
    {
        [ServiceDependency]
        public ICircularDependencyA DependencyA { get; private set; }
    }
    public class CircularDependencyD : ICircularDependencyD
    {
        [ServiceDependency]
        public ICircularDependencyC DependencyC { get; private set; }
    }
    public class CircularDependencyE : ICircularDependencyE
    {
        [ServiceDependency]
        public ICircularDependencyE DependencyD { get; private set; }
    }

    // Fails because the implementation's service dependency is missing a private setter.
    public interface IMissingPrivateDependencySetter {}
    public class MissingPrivateDependencySetter : IMissingPrivateDependencySetter
    {
        [ServiceDependency]
        public ITestService ATestService { get; }
    }

    // Fails because the implementation's service dependency is not an interface.
    public interface IInvalidServiceDependencyType {}
    public class InvalidServiceDependencyType : IInvalidServiceDependencyType
    {
        [ServiceDependency]
        public TestService ATestService { get; private set; }
    }
}
