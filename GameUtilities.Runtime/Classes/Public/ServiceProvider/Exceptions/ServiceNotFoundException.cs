namespace ADM87.GameUtilities.Services
{
    using System;

    /// <summary>
    /// Represents an exception that is thrown when a service with the specified identity type is not found in the service collection.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ServiceNotFoundException"/> class.
    /// </remarks>
    public sealed class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(Type identityType)
            : base($"The service with the identity type '{identityType.Name}' was not found in the service collection.") {}
    }
}