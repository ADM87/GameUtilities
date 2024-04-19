using ADM87.GameUtilities.ServiceProvider;

namespace GameUtilities.Tests
{
    public interface ICollectedService {}
    [ServiceDefinition(typeof(ICollectedService))]
    public class CollectedService : ICollectedService {}

    public interface ICollectedSingletonService {}
    [ServiceDefinition(typeof(ICollectedSingletonService), isSingleton: true)]
    public class CollectedSingletonService : ICollectedSingletonService {}

    public interface ICollectedServiceWidthDependency
    {
        ICollectedService ASimpleCollectedService { get; }
    }
    [ServiceDefinition(typeof(ICollectedServiceWidthDependency))]
    public class CollectedServiceWithDependency : ICollectedServiceWidthDependency
    {
        [ServiceDependency]
        public ICollectedService ASimpleCollectedService { get; private set; }
    }

    public interface IManualService {}
    public class ManualService : IManualService {}

    public interface IManualSingletonService {}
    public class ManualSingletonService : IManualSingletonService {}

    public interface IManualServiceWidthDependency
    {
        IManualService ASimpleManualService { get; }
    }
    public class ManualServiceWithDependency : IManualServiceWidthDependency
    {
        [ServiceDependency]
        public IManualService ASimpleManualService { get; private set; }
    }
}
