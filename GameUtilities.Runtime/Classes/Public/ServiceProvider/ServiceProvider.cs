using System;
using System.Collections.Generic;
using System.Reflection;

namespace ADM87.GameUtilities.Services
{
    using ServiceCollection = Dictionary<Type, ServiceDefinition>;

    /// <summary>
    /// Provides a collection of services and methods for managing and retrieving them.
    /// </summary>
    public static class ServiceProvider
    {
        internal static ServiceCollection Collection { get; } = new ServiceCollection();

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

                    AddService(attribute.IdentityType, type, attribute.LifeTime);
                }
            }

            // Create instances of singleton services.
            foreach (ServiceDefinition definition in Collection.Values)
            {
                if (definition.ServiceLifeTime == EServiceLifeTime.Singleton)
                    Get(definition.Identity);
            }
        }

        public static void AddService<TIdentity, TImplementation>(EServiceLifeTime serviceLifeTime = EServiceLifeTime.Transient)
        {
            AddService(typeof(TIdentity), typeof(TImplementation), serviceLifeTime);
        }

        public static void AddService(Type identityType,
                                      Type implementationType,
                                      EServiceLifeTime serviceLifeTime = EServiceLifeTime.Transient)
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
                ServiceLifeTime   = serviceLifeTime
            });

            // Check for circular dependencies.
            ResolveDependencies(identityType);
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

            if (definition.ServiceLifeTime == EServiceLifeTime.Singleton)
                definition.Instance = instance;

            foreach (PropertyInfo property in definition.Dependencies)
                property.SetValue(instance, Get(property.PropertyType));

            return instance;
        }

        /// <summary>
        /// Injects dependencies into the specified target object.
        /// </summary>
        /// <param name="target"></param>
        public static void InjectDependencies(object target)
        {
            IEnumerable<PropertyInfo> properties = GetServiceDependencies(target.GetType());
            foreach (PropertyInfo property in properties)
                property.SetValue(target, Get(property.PropertyType));
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
            List<PropertyInfo> dependencies = new List<PropertyInfo>();
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

        private static void ResolveDependencies(Type identityType)
        {
            foreach (PropertyInfo property in Collection[identityType].Dependencies)
            {
                if (Collection.TryGetValue(property.PropertyType, out ServiceDefinition dependency))
                    ResolveDependencies(dependency, new List<Type>() { identityType });
            }
        }

        /// <summary>
        /// Resolves the dependencies for a given service definition.
        /// </summary>
        /// <param name="definition">The service definition.</param>
        /// <param name="dependencyChain">The dependency chain to track circular dependencies.</param>
        private static void ResolveDependencies(ServiceDefinition definition,
                                                List<Type> dependencyChain)
        {
            // If the dependency chain already contains the service implementation, we have a circular dependency.
            if (dependencyChain.Contains(definition.Identity))
                throw new CircularServiceDependencyException(dependencyChain);

            // If the definition is a singleton, we don't need to check its dependencies. Their instances are already created.
            if (definition.ServiceLifeTime == EServiceLifeTime.Singleton)
                return;

            dependencyChain.Add(definition.Identity);

            foreach (PropertyInfo property in definition.Dependencies)
            {
                if (Collection.TryGetValue(property.PropertyType, out ServiceDefinition dependency))
                    ResolveDependencies(dependency, dependencyChain);
            }

            dependencyChain.Remove(definition.Identity);
        }
    }
}
