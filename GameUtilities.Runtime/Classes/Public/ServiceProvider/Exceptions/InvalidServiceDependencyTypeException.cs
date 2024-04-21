using System;
using System.Reflection;

namespace ADM87.GameUtilities.ServiceProvider
{
    /// <summary>
    /// Represents an exception that is thrown when a service dependency is of an invalid type.
    /// </summary>
    public sealed class InvalidServiceDependencyTypeException : Exception
    {
        public InvalidServiceDependencyTypeException(Type implementationType, PropertyInfo property)
            : base($"Service dependency {implementationType.Name}.{property.Name} must be an interface type") {}
    }
}