using System;

namespace ADM87.GameUtilities.ServiceProvider
{
    /// <summary>
    /// Defines an implementation for the provided service identity.
    /// </summary>
    /// <param name="identitiyType">Must be an interface type</param>
    /// <param name="isSingleton"></param>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServiceDefinitionAttribute(Type identitiyType, bool isSingleton = false) : Attribute
    {
        public Type IdentityType { get; private set; } = identitiyType;
        public bool IsSingleton { get; private set; } = isSingleton;
    }
}
