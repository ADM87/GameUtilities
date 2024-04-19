using System;

namespace ADM87.GameUtilities.ServiceProvider
{
    /// <summary>
    /// Represents an exception that is thrown when duplicate service identities are detected.
    /// </summary>
    /// <param name="identityType">The type of the duplicate service identity.</param>
    public sealed class DuplicateServiceIdentityException(Type identityType)
        : Exception($"Detected duplicate service identities, {identityType.Name}") {}
}
