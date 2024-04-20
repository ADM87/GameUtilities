using System.Reflection;

namespace ADM87.GameUtilities.ServiceProvider
{
    using ServiceCollection = Dictionary<Type, ServiceDefinition>;

    /// <summary>
    /// Provides a collection of services and methods for managing and retrieving them.
    /// </summary>
    public static class Services
    {
        internal static ServiceCollection Collection { get; } = [];

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

                    AddService(attribute.IdentityType, type, attribute.IsSingleton);
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

            if (Collection.ContainsKey(identityType))
                throw new DuplicateServiceIdentityException(identityType);

            foreach (var kvp in Collection)
            {
                if (kvp.Value.Implementation.Equals(implementationType))
                    throw new DuplicateServiceImplementationException(identityType, implementationType);
            }

            Collection.Add(identityType, new ServiceDefinition {
                Identity        = identityType,
                Implementation  = implementationType,
                Dependencies    = GetServiceDependencies(implementationType),
                IsSingleton     = isSingleton
            });
        }

        /// <summary>
        /// Gets an instance of the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the instance to retrieve.</typeparam>
        /// <returns>An instance of the specified type <typeparamref name="T"/>.</returns>
        public static T Get<T>()
            => (T)Get(typeof(T));


        /// <summary>
        /// Retrieves an instance of the specified type from the service provider.
        /// </summary>
        /// <param name="type">The type of the instance to retrieve.</param>
        /// <returns>An instance of the specified type.</returns>
        public static object Get(Type type)
        {
            ServiceDefinition definition = GetServiceDefinition(type);

            if (definition.Instance != null)
                return definition.Instance;

            object instance = Activator.CreateInstance(definition.Implementation);

            if (definition.IsSingleton)
                definition.Instance = instance;

            Stack<Type> dependencyChain = new Stack<Type>();
            dependencyChain.Push(definition.Implementation);

            foreach (PropertyInfo property in definition.Dependencies)
            {
                ResolveDependencies(GetServiceDefinition(property.PropertyType), dependencyChain);
                property.SetValue(instance, Get(property.PropertyType));
            }

            return instance;
        }

        /// <summary>
        /// Resolves the dependencies for a given service definition.
        /// </summary>
        /// <param name="definition">The service definition.</param>
        /// <param name="dependencyChain">The dependency chain to track circular dependencies.</param>
        private static void ResolveDependencies(ServiceDefinition definition,
                                                Stack<Type> dependencyChain)
        {
            // If the dependency chain already contains the service identity, we have a circular dependency.
            if (dependencyChain.Contains(definition.Identity))
                throw new CircularServiceDependencyException(dependencyChain);

            // If the service is a singleton, we don't need to check its dependencies. Their instances are already created.
            if (definition.IsSingleton)
                return;

            dependencyChain.Push(definition.Implementation);

            foreach (PropertyInfo property in definition.Dependencies)
                ResolveDependencies(GetServiceDefinition(property.PropertyType), dependencyChain);

            dependencyChain.Pop();
        }

        /// <summary>
        /// Returns a definition for a service.
        /// </summary>
        private static ServiceDefinition GetServiceDefinition(Type type)
        {
            if (!Collection.TryGetValue(type, out ServiceDefinition definition))
                throw new ServiceNotFoundException(type);

            return definition;
        }

        /// <summary>
        /// Retrieves the service dependencies of a given implementation type.
        /// </summary>
        /// <param name="implementationType">The type of the implementation.</param>
        /// <returns>An enumerable collection of <see cref="PropertyInfo"/> objects representing the service dependencies.</returns>
        /// <exception cref="MissingServiceDependencySetterException">Thrown when a service dependency property does not have a private setter.</exception>
        /// <exception cref="InvalidServiceDependencyTypeException">Thrown when a service dependency property is not of an interface type.</exception>
        private static IEnumerable<PropertyInfo> GetServiceDependencies(Type implementationType)
        {
            IEnumerable<PropertyInfo> properties = implementationType.GetProperties(BindingFlags.Instance
                                                                                  | BindingFlags.NonPublic
                                                                                  | BindingFlags.Public
                                                                                  | BindingFlags.DeclaredOnly);
            List<PropertyInfo> dependencies = [];
            foreach (PropertyInfo property in properties)
            {
                if (property.GetCustomAttribute<ServiceDependencyAttribute>() == null)
                    continue;

                if (property.GetSetMethod(true) == null)
                    throw new MissingServiceDependencySetterException(implementationType, property);

                if (!property.PropertyType.IsInterface)
                    throw new InvalidServiceDependencyTypeException(implementationType, property);

                dependencies.Add(property);
            }
            return dependencies;
        }
    }
}
