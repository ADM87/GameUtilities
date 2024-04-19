using System.Reflection;

namespace ADM87.GameUtilities.ServiceProvider
{
    /// <summary>
    /// Represents an exception that is thrown when a service dependency is of an invalid type.
    /// </summary>
    /// <param name="implementationType">The type of the implementation that has the invalid service dependency.</param>
    /// <param name="property">The property that represents the invalid service dependency.</param>
    public sealed class InvalidServiceDependencyTypeException(Type implementationType, PropertyInfo property)
        : Exception($"Service dependency {implementationType.Name}.{property.Name} must be an interface type") {}
}