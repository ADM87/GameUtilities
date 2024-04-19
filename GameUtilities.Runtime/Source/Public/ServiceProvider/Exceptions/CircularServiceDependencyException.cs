namespace ADM87.GameUtilities.ServiceProvider
{
    /// <summary>
    /// Represents an exception that is thrown when a circular dependency is detected in a service provider.
    /// </summary>
    /// <param name="implementationType">The type of the service implementation that has the circular dependency.</param>
    /// <param name="dependencyType">The type of the service dependency that causes the circular dependency.</param>
    public sealed class CircularServiceDependencyException(Type implementationType, Type dependencyType)
        : Exception($"Detected circular dependency for {implementationType.Name} > {dependencyType.Name}") {}
}