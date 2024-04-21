using System;
using System.Reflection;

namespace ADM87.GameUtilities.ServiceProvider
{
    /// <summary>
    /// Represents an exception that is thrown when a service dependency does not have a private setter.
    /// </summary>
    /// <param name="implementationType">The type of the implementation that has the missing service dependency setter.</param>
    /// <param name="property">The property that represents the missing service dependency setter.</param>
    public sealed class MissingServiceDependencySetterException : Exception
    {
        public MissingServiceDependencySetterException(Type implementationType, PropertyInfo property)
            : base($"Service dependency {implementationType.Name}.{property.Name} must have a private setter") {}
    }
}