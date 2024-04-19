using System.Reflection;
using ADM87.GameUtilities.Linq;

namespace ADM87.GameUtilities.ServiceProvider
{
    using ServiceCollection = Dictionary<Type, ServiceDefinition>;

    public static class Services
    {
        private static readonly ServiceCollection k_serviceCollection = new ServiceCollection();

        internal static ServiceCollection Collection => k_serviceCollection;

        /// <summary>
        /// Collects service definitions from all assemblies in the current application domain.
        /// </summary>
        public static void CollectServiceDefinitions()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsAbstract || type.IsInterface)
                        continue;

                    ServiceDefinitionAttribute attribute = type.GetCustomAttribute<ServiceDefinitionAttribute>();

                    if (attribute == null)
                        continue;

                    AddServiceInternal(attribute.IdentityType, type, attribute.IsSingleton);
                }
            }
        }

        /// <summary>
        /// Adds a service to the service provider.
        /// </summary>
        /// <typeparam name="TIdentity">The type of the service identity.</typeparam>
        /// <typeparam name="TImplementation">The type of the service implementation.</typeparam>
        /// <param name="isSingleton">Indicates whether the service should be registered as a singleton (default is false).</param>
        public static void AddService<TIdentity, TImplementation>(bool isSingleton = false)
        {
            AddService(typeof(TIdentity), typeof(TImplementation), isSingleton);
        }

        /// <summary>
        /// Adds a service to the service collection.
        /// </summary>
        /// <param name="identityType">The type of the service identity.</param>
        /// <param name="implementationType">The type of the service implementation.</param>
        /// <param name="isSingleton">A flag indicating whether the service should be treated as a singleton.</param>
        /// <exception cref="InvalidServiceIdentityTypeException">Thrown when the <paramref name="identityType"/> is not an interface.</exception>
        /// <exception cref="InvalidServiceImplementationException">Thrown when the <paramref name="implementationType"/> does not implement the <paramref name="identityType"/>.</exception>
        /// <exception cref="DuplicateServiceIdentityException">Thrown when a service with the same <paramref name="identityType"/> already exists in the service collection.</exception>
        /// <exception cref="DuplicateServiceImplementationException">Thrown when a service with the same <paramref name="implementationType"/> already exists in the service collection.</exception>
        public static void AddService(Type identityType,
                                      Type implementationType,
                                      bool isSingleton)
        {
            if (!identityType.IsInterface)
                throw new InvalidServiceIdentityTypeException(identityType, implementationType);

            if (!identityType.IsAssignableFrom(implementationType))
                throw new InvalidServiceImplementationException(identityType, implementationType);

            if (k_serviceCollection.ContainsKey(identityType))
                throw new DuplicateServiceIdentityException(identityType);

            k_serviceCollection.ForEach(kvp => {
                if (kvp.Value.Implementation.Equals(implementationType))
                    throw new DuplicateServiceImplementationException(identityType, implementationType);
            });

            k_serviceCollection.Add(identityType, new ServiceDefinition {
                Identity        = identityType,
                Implementation  = implementationType,
                Dependencies    = GetServiceDependencies(implementationType),
                IsSingleton     = isSingleton
            });
        }

        /// <summary>
        /// Retrieves the service dependencies for a given implementation type.
        /// </summary>
        /// <param name="implementationType">The implementation type to retrieve service dependencies for.</param>
        /// <returns>An enumerable collection of PropertyInfo objects representing the service dependencies.</returns>
        private static IEnumerable<PropertyInfo> GetServiceDependencies(Type implementationType)
        {
            return implementationType
                .GetProperties()
                .Where(property => {
                    if (property.GetCustomAttribute<ServiceDependencyAttribute>() == null)
                        return false;

                    if (property.GetSetMethod(true) == null)
                        throw new MissingServiceDependencySetterException(implementationType, property);

                    if (!property.PropertyType.IsInterface)
                        throw new InvalidServiceDependencyTypeException(implementationType, property);

                    return true;
                });
        }
    }
}
