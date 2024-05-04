using System;

namespace ADM87.GameUtilities.Services
{
    /// <summary>
    /// Represents an exception that is thrown when duplicate service implementations are detected.
    /// </summary>
    /// <param name="identityType">The type of the service identity.</param>
    /// <param name="implementationType">The type of the duplicate service implementation.</param>
    public sealed class DuplicateServiceImplementationException : Exception
    {
        public DuplicateServiceImplementationException(Type identityType, Type implementationType)
            : base($"Detected duplicate service implementations {implementationType.Name} for identity {identityType.Name}") {}
    }
}