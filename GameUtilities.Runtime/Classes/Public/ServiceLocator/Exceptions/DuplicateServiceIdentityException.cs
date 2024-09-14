using System;

namespace ADM87.GameUtilities.Services
{
    /// <summary>
    /// Represents an exception that is thrown when duplicate service identities are detected.
    /// </summary>
    /// <param name="identityType">The type of the duplicate service identity.</param>
    public sealed class DuplicateServiceIdentityException : Exception
    {
        public DuplicateServiceIdentityException(Type identityType)
            : base($"Detected duplicate service identities, {identityType.Name}") {}
    }
}
