using ADM87.GameUtilities.Async;
using ADM87.GameUtilities.ServiceProvider;

namespace GameUtilities.Tests
{
    /// <summary>
    /// This class contains unit tests for the AsyncService class.
    /// </summary>
    [TestFixture]
    public class PassAsyncTests
    {
        /// <summary>
        /// Tests the automated collection of the AsyncService definition.
        /// </summary>
        [Test, Order(1)]
        public static void PassAsyncService()
        {
            Services.Collection.Clear();
            Services.CollectServiceDefinitions();
            Assert.Multiple(() =>
            {
                Assert.That(Services.Collection,                                    Is.Not.Empty);
                Assert.That(Services.Collection.ContainsKey(typeof(IAsyncService)), Is.True);
                Assert.That(Services.Collection[typeof(IAsyncService)].IsSingleton, Is.True);
                Assert.That(Services.Get<IAsyncService>(),                          Is.TypeOf<AsyncService>());
            });
        }

        /// <summary>
        /// Tests the RunAsync method with a successful operation.
        /// </summary>
        /// <returns></returns>
        [Test, Order(2)]
        public static async Task PassRunAsyncCompleted()
        {
            Services.Collection.Clear();
            Services.CollectServiceDefinitions();
            IAsyncService asyncService = Services.Get<IAsyncService>();
            AsyncOperationHandle handle = asyncService.RunAsync(PassAsyncMocks.SuccessAsyncOperation);
            handle.Completed.Connect(() => {
                Assert.That(handle.Phase, Is.EqualTo(EAsyncOperationPhase.Completed));
            });
            handle.Cancelled.Connect(() => {
                Assert.Fail("Operation was cancelled.");
            });
            handle.Faulted.Connect((Exception ex) => {
                Assert.Fail($"Operation failed with exception: {ex.Message}");
            });
            await handle.OperationTask;
        }

        /// <summary>
        /// Tests the RunAsync method with a cancelled operation.
        /// </summary>
        /// <returns></returns>
        [Test, Order(3)]
        public static async Task PassRunAsyncCancelled()
        {
            Services.Collection.Clear();
            Services.CollectServiceDefinitions();
            IAsyncService asyncService = Services.Get<IAsyncService>();
            AsyncOperationHandle handle = asyncService.RunAsync(PassAsyncMocks.CancelledAsyncOperation);
            handle.Completed.Connect(() => {
                Assert.Fail("Operation was completed.");
            });
            handle.Cancelled.Connect(() => {
                Assert.That(handle.Phase, Is.EqualTo(EAsyncOperationPhase.Cancelled));
            });
            handle.Faulted.Connect((Exception ex) => {
                Assert.Fail($"Operation failed with exception: {ex.Message}");
            });
            await Task.Delay(100);
            handle.Cancel();
            await handle.OperationTask;
        }

        /// <summary>
        /// Tests the RunAsync method with a faulted operation.
        /// </summary>
        /// <returns></returns>
        [Test, Order(4)]
        public static async Task PassRunAsyncFaulted()
        {
            Services.Collection.Clear();
            Services.CollectServiceDefinitions();
            IAsyncService asyncService = Services.Get<IAsyncService>();
            AsyncOperationHandle handle = asyncService.RunAsync(PassAsyncMocks.FaultedAsyncOperation);
            handle.Completed.Connect(() => {
                Assert.Fail("Operation was completed.");
            });
            handle.Cancelled.Connect(() => {
                Assert.Fail("Operation was cancelled.");
            });
            handle.Faulted.Connect((Exception ex) => {
                Assert.That(handle.Phase, Is.EqualTo(EAsyncOperationPhase.Faulted));
                Assert.That(ex, Is.TypeOf<PassAsyncMocks.AsyncTestException>());
            });
            await handle.OperationTask;
        }
    }
}
