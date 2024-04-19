using ADM87.GameUtilities.ServiceProvider;

namespace GameUtilities.Tests
{
    public interface ISimpleCollectedService {}
    [ServiceDefinition(typeof(ISimpleCollectedService))]
    public class SimpleCollectedService : ISimpleCollectedService {}

    public interface ICollectedSingletonService {}
    [ServiceDefinition(typeof(ICollectedSingletonService), isSingleton: true)]
    public class CollectedSingletonService : ICollectedSingletonService {}

    public interface ISimpleManualService {}
    public class SimpleManualService : ISimpleManualService {}

    public interface IManualServiceWidthDependency
    {
        ISimpleManualService ASimpleManualService { get; }
    }
    public class ManualServiceWithDependency : IManualServiceWidthDependency
    {
        [ServiceDependency]
        public ISimpleManualService ASimpleManualService { get; private set; }
    }
}
