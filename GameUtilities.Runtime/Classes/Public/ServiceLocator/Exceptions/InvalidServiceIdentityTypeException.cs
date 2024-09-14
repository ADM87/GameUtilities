using System;

namespace ADM87.GameUtilities.Services
{
    /// <summary>
    /// Represents an exception that is thrown when the service identity type is invalid.
    /// </summary>
    /// <param name="identityType">The type of the service identity.</param>
    /// <param name="implementationType">The type of the service implementation.</param>
    public sealed class InvalidServiceIdentityTypeException : Exception
    {
        public InvalidServiceIdentityTypeException(Type identityType, Type implementationType)
            : base($"Service identity type for {implementationType.Name} must be an interface, {identityType.Name} ") {}
    }
}
