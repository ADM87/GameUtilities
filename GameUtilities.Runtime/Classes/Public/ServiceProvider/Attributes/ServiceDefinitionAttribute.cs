using System;

namespace ADM87.GameUtilities.Services
{
    /// <summary>
    /// Defines an implementation for the provided service identity.
    /// </summary>
    /// <param name="identitiyType">Must be an interface type</param>
    /// <param name="isSingleton"></param>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServiceDefinitionAttribute : Attribute
    {
        public Type IdentityType { get; private set; }
        public bool IsSingleton { get; private set; }

        public ServiceDefinitionAttribute(Type identitiyType, bool isSingleton = false)
        {
            IdentityType = identitiyType;
            IsSingleton = isSingleton;
        }
    }
}
