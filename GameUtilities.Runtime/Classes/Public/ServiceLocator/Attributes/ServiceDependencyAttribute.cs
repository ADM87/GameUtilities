using System;

namespace ADM87.GameUtilities.Services
{
    /// <summary>
    /// Represents an attribute that marks a property as a dependency on a service.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ServiceDependencyAttribute : Attribute {}
}
