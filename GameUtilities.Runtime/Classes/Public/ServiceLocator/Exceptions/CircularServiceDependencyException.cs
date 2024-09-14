using System;
using System.Collections.Generic;
using System.Linq;

namespace ADM87.GameUtilities.Services
{
    /// <summary>
    /// Represents an exception that is thrown when circular dependencies are detected in a service dependency chain.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CircularServiceDependencyException"/> class with the specified dependency chain.
    /// </remarks>
    /// <param name="dependencyChain">The list of types representing the dependency chain.</param>
    public sealed class CircularServiceDependencyException : Exception
    {
        public CircularServiceDependencyException(IEnumerable<Type> dependencyChain)
            : base($"Detected circular dependencies: {string.Join(">", dependencyChain.Select(dep => dep.Name))}") {}
    }
}
