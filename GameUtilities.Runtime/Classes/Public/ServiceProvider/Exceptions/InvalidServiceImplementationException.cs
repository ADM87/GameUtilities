using System;

namespace ADM87.GameUtilities.ServiceProvider
{
    /// <summary>
    /// Represents an exception that is thrown when a service implementation does not implement the required identity type.
    /// </summary>
    /// <param name="identityType">The type of the required identity.</param>
    /// <param name="implementationType">The type of the service implementation.</param>
    public sealed class InvalidServiceImplementationException : Exception
    {
        public InvalidServiceImplementationException(Type identityType, Type implementationType)
            : base($"Service implementation {implementationType.Name} must implement {identityType.Name}") {}
    }
}
