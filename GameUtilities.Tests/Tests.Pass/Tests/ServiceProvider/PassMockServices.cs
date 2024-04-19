using ADM87.GameUtilities.ServiceProvider;

namespace GameUtilities.Tests
{
    public interface IPassCollectService {}
    [ServiceDefinition(typeof(IPassCollectService))]
    public class PassCollectService : IPassCollectService {}

    public interface IPassCollectSingletonService {}
    [ServiceDefinition(typeof(IPassCollectSingletonService), isSingleton: true)]
    public class PassCollectSingletonService : IPassCollectSingletonService {}

    public interface ISimplePassService {}
    public class SimplePassService : ISimplePassService {}

    public interface IPassServiceWithDependency {}
    public class PassServiceWithDependency : IPassServiceWithDependency
    {
        [ServiceDependency]
        public ISimplePassService aSimplePassService { get; private set; }
    }
}