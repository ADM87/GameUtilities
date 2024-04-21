using ADM87.GameUtilities.ServiceProvider;

namespace GameUtilities.Tests
{
    public interface ICollectedService {}
    [ServiceDefinition(typeof(ICollectedService))]
    public class CollectedService : ICollectedService {}

    public interface ICollectedSingletonService {}
    [ServiceDefinition(typeof(ICollectedSingletonService), isSingleton: true)]
    public class CollectedSingletonService : ICollectedSingletonService {}

    public interface ICollectedServiceWithDependency
    {
        ICollectedService ASimpleCollectedService { get; }
    }
    [ServiceDefinition(typeof(ICollectedServiceWithDependency))]
    public class CollectedServiceWithDependency : ICollectedServiceWithDependency
    {
        [ServiceDependency]
        public ICollectedService ASimpleCollectedService { get; private set; }
    }

    public interface IManualService {}
    public class ManualService : IManualService {}

    public interface IManualSingletonService {}
    public class ManualSingletonService : IManualSingletonService {}

    public interface IManualServiceWithDependency
    {
        IManualService ASimpleManualService { get; }
    }
    public class ManualServiceWithDependency : IManualServiceWithDependency
    {
        [ServiceDependency]
        public IManualService ASimpleManualService { get; private set; }
    }

    public interface IManualServiceWithSingletonDependency
    {
        IManualSingletonService ASingletonManualService { get; }
    }
    public class ManualServiceWithSingletonDependency : IManualServiceWithSingletonDependency
    {
        [ServiceDependency]
        public IManualSingletonService ASingletonManualService { get; private set; }
    }
}
