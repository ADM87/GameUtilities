using ADM87.GameUtilities.Async;
using ADM87.GameUtilities.ServiceProvider;

namespace GameUtilities.Tests
{
    /// <summary>
    /// This class contains unit tests for the AsyncService class.
    /// </summary>
    [TestFixture]
    public class FailAsyncTests
    {
        [Test, Order(1)]
        public static async Task FailInvalidAsyncHandleStart()
        {
            Services.Collection.Clear();
            Services.CollectServiceDefinitions();
            IAsyncService asyncService = Services.Get<IAsyncService>();
            AsyncOperationHandle handle = asyncService.RunAsync(async ct => await Task.Delay(1000));
            Assert.Throws<InvalidAsyncHandleOperation>(() => handle.Start());
            await handle.OperationTask;
        }
    }
}
