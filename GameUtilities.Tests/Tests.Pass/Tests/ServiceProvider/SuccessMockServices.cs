using ADM87.GameUtilities.ServiceProvider;

namespace GameUtilities.Tests
{
    public interface ISuccessCollectService {}
    [ServiceDefinition(typeof(ISuccessCollectService))]
    public class SuccessCollectService : ISuccessCollectService {}

    public interface ISuccessCollectSingletonService {}
    [ServiceDefinition(typeof(ISuccessCollectSingletonService), isSingleton: true)]
    public class SuccessCollectSingletonService : ISuccessCollectSingletonService {}

    public interface ISimpleSuccessService {}
    public class SimpleSuccessService : ISimpleSuccessService {}

    public interface ISuccessServiceWithDependency {}
    public class SuccessServiceWithDependency : ISuccessServiceWithDependency
    {
        [ServiceDependency]
        public ISimpleSuccessService aSimpleSuccessService { get; private set; }
    }
}
