using ADM87.GameUtilities.Services;

namespace GameUtilities.Tests
{
    [TestFixture]
    public class PassAsyncOpTests()
    {
        [Test, Order(1)]
        public void PassAsyncOpServiceExists()
        {
            ServiceLocator.CollectServiceDefinitions();

            IAsyncOperationService asyncOpService = ServiceLocator.GetService<IAsyncOperationService>();

            Assert.Multiple(() =>
            {
                Assert.That(asyncOpService, Is.Not.Null);
                Assert.That(asyncOpService, Is.InstanceOf<AsyncOperationService >());
            });
        }

        [Test, Order(2)]
        public void PassAsyncOpServiceRun()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IAsyncOperationService, AsyncOperationService>();

            IAsyncOperationService asyncOpService = ServiceLocator.GetService<IAsyncOperationService>();
            IAsyncOperationHandle asyncOpHandle = asyncOpService.RunAsync((token) => Task.Delay(1000, token));

            // Simulate an update loop that updates the async operation service.
            do {
                Thread.Sleep(30);
                asyncOpService.UpdateOperationHandles();
            } while (asyncOpHandle.IsRunning);

            Assert.Multiple(() =>
            {
                Assert.That(asyncOpHandle.IsCompleted, Is.True);
                Assert.That(asyncOpHandle.IsCanceled, Is.False);
                Assert.That(asyncOpHandle.IsFailed, Is.False);
                Assert.That(AsyncOperationService.OperationHandles, Is.Empty);
            });
        }

        [Test, Order(3)]
        public void PassAsyncOpServiceCancel()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IAsyncOperationService, AsyncOperationService>();

            IAsyncOperationService asyncOpService = ServiceLocator.GetService<IAsyncOperationService>();
            IAsyncOperationHandle asyncOpHandle = asyncOpService.RunAsync((token) => Task.Delay(1000, token));

            asyncOpHandle.Cancel();

            // Simulate an update loop that updates the async operation service.
            do {
                Thread.Sleep(30);
                asyncOpService.UpdateOperationHandles();
            } while (asyncOpHandle.IsRunning);

            Assert.Multiple(() =>
            {
                Assert.That(asyncOpHandle.IsCompleted, Is.False);
                Assert.That(asyncOpHandle.IsCanceled, Is.True);
                Assert.That(asyncOpHandle.IsFailed, Is.False);
                Assert.That(AsyncOperationService.OperationHandles, Is.Empty);
            });
        }

        [Test, Order(4)]
        public void PassAsyncOpServiceCompleteEmit()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IAsyncOperationService, AsyncOperationService>();

            IAsyncOperationService asyncOpService = ServiceLocator.GetService<IAsyncOperationService>();
            IAsyncOperationHandle asyncOpHandle = asyncOpService.RunAsync((token) => Task.Delay(1000, token));

            bool completed = false;
            asyncOpHandle.OnCompleted.Connect(() => completed = true);

            // Simulate an update loop that updates the async operation service.
            do {
                Thread.Sleep(30);
                asyncOpService.UpdateOperationHandles();
            } while (asyncOpHandle.IsRunning);

            Assert.Multiple(() =>
            {
                Assert.That(asyncOpHandle.IsCompleted, Is.True);
                Assert.That(asyncOpHandle.IsCanceled, Is.False);
                Assert.That(asyncOpHandle.IsFailed, Is.False);
                Assert.That(completed, Is.True);
                Assert.That(AsyncOperationService.OperationHandles, Is.Empty);
            });
        }

        [Test, Order(5)]
        public void PassAsyncOpServiceCancelEmit()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IAsyncOperationService, AsyncOperationService>();

            IAsyncOperationService asyncOpService = ServiceLocator.GetService<IAsyncOperationService>();
            IAsyncOperationHandle asyncOpHandle = asyncOpService.RunAsync((token) => Task.Delay(1000, token));

            bool canceled = false;

            asyncOpHandle.OnCanceled.Connect(() => canceled = true);
            asyncOpHandle.Cancel();

            // Simulate an update loop that updates the async operation service.
            do {
                Thread.Sleep(30);
                asyncOpService.UpdateOperationHandles();
            } while (asyncOpHandle.IsRunning);

            Assert.Multiple(() =>
            {
                Assert.That(asyncOpHandle.IsCompleted, Is.False);
                Assert.That(asyncOpHandle.IsCanceled, Is.True);
                Assert.That(asyncOpHandle.IsFailed, Is.False);
                Assert.That(canceled, Is.True);
                Assert.That(AsyncOperationService.OperationHandles, Is.Empty);
            });
        }

        [Test, Order(6)]
        public void PassAsyncOpServiceFailEmit()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IAsyncOperationService, AsyncOperationService>();

            IAsyncOperationService asyncOpService = ServiceLocator.GetService<IAsyncOperationService>();
            IAsyncOperationHandle asyncOpHandle = asyncOpService.RunAsync((token) => Task.FromException(new Exception("Test exception.")));

            bool failed = false;
            asyncOpHandle.OnFailed.Connect((ex) => failed = true);

            // Simulate an update loop that updates the async operation service.
            do {
                Thread.Sleep(30);
                asyncOpService.UpdateOperationHandles();
            } while (asyncOpHandle.IsRunning);

            Assert.Multiple(() =>
            {
                Assert.That(asyncOpHandle.IsCompleted, Is.False);
                Assert.That(asyncOpHandle.IsCanceled, Is.False);
                Assert.That(asyncOpHandle.IsFailed, Is.True);
                Assert.That(failed, Is.True);
                Assert.That(AsyncOperationService.OperationHandles, Is.Empty);
            });
        }

        [Test, Order(7)]
        public void PassAsyncOpServiceRunWithResult()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IAsyncOperationService, AsyncOperationService>();

            IAsyncOperationService asyncOpService = ServiceLocator.GetService<IAsyncOperationService>();
            IAsyncOperationHandle<int> asyncOpHandle = asyncOpService.RunAsync(token => Task.FromResult(42));

            // Simulate an update loop that updates the async operation service.
            do {
                Thread.Sleep(30);
                asyncOpService.UpdateOperationHandles();
            } while (asyncOpHandle.IsRunning);

            Assert.Multiple(() =>
            {
                Assert.That(asyncOpHandle.IsCompleted, Is.True);
                Assert.That(asyncOpHandle.IsCanceled, Is.False);
                Assert.That(asyncOpHandle.IsFailed, Is.False);
                Assert.That(asyncOpHandle.Result, Is.EqualTo(42));
                Assert.That(AsyncOperationService.OperationHandles, Is.Empty);
            });
        }

        [Test, Order(8)]
        public void PassAsyncOpServiceCancelWithResult()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IAsyncOperationService, AsyncOperationService>();

            IAsyncOperationService asyncOpService = ServiceLocator.GetService<IAsyncOperationService>();
            IAsyncOperationHandle<int> asyncOpHandle = asyncOpService.RunAsync(token => Task.FromResult(42));

            asyncOpHandle.Cancel();

            // Simulate an update loop that updates the async operation service.
            do {
                Thread.Sleep(30);
                asyncOpService.UpdateOperationHandles();
            } while (asyncOpHandle.IsRunning);

            Assert.Multiple(() =>
            {
                Assert.That(asyncOpHandle.IsCompleted, Is.False);
                Assert.That(asyncOpHandle.IsCanceled, Is.True);
                Assert.That(asyncOpHandle.IsFailed, Is.False);
                Assert.That(asyncOpHandle.Result, Is.EqualTo(0));
                Assert.That(AsyncOperationService.OperationHandles, Is.Empty);
            });
        }

        [Test, Order(9)]
        public void PassAsyncOpServiceCompleteEmitWithResult()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IAsyncOperationService, AsyncOperationService>();

            IAsyncOperationService asyncOpService = ServiceLocator.GetService<IAsyncOperationService>();
            IAsyncOperationHandle<int> asyncOpHandle = asyncOpService.RunAsync(token => Task.FromResult(42));

            bool completed = false;
            asyncOpHandle.OnCompleted.Connect((result) => completed = true);

            // Simulate an update loop that updates the async operation service.
            do {
                Thread.Sleep(30);
                asyncOpService.UpdateOperationHandles();
            } while (asyncOpHandle.IsRunning);

            Assert.Multiple(() =>
            {
                Assert.That(asyncOpHandle.IsCompleted, Is.True);
                Assert.That(asyncOpHandle.IsCanceled, Is.False);
                Assert.That(asyncOpHandle.IsFailed, Is.False);
                Assert.That(completed, Is.True);
                Assert.That(asyncOpHandle.Result, Is.EqualTo(42));
                Assert.That(AsyncOperationService.OperationHandles, Is.Empty);
            });
        }

        [Test, Order(10)]
        public void PassAsyncOpServiceCancelEmitWithResult()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IAsyncOperationService, AsyncOperationService>();

            IAsyncOperationService asyncOpService = ServiceLocator.GetService<IAsyncOperationService>();
            IAsyncOperationHandle<int> asyncOpHandle = asyncOpService.RunAsync(token => Task.FromResult(42));

            bool canceled = false;

            asyncOpHandle.OnCanceled.Connect(() => canceled = true);
            asyncOpHandle.Cancel();

            // Simulate an update loop that updates the async operation service.
            do {
                Thread.Sleep(30);
                asyncOpService.UpdateOperationHandles();
            } while (asyncOpHandle.IsRunning);

            Assert.Multiple(() =>
            {
                Assert.That(asyncOpHandle.IsCompleted, Is.False);
                Assert.That(asyncOpHandle.IsCanceled, Is.True);
                Assert.That(asyncOpHandle.IsFailed, Is.False);
                Assert.That(canceled, Is.True);
                Assert.That(asyncOpHandle.Result, Is.EqualTo(0));
                Assert.That(AsyncOperationService.OperationHandles, Is.Empty);
            });
        }

        [Test, Order(11)]
        public void PassAsyncOpServiceFailEmitWithResult()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IAsyncOperationService, AsyncOperationService>();

            IAsyncOperationService asyncOpService = ServiceLocator.GetService<IAsyncOperationService>();
            IAsyncOperationHandle<int> asyncOpHandle = asyncOpService.RunAsync(token => Task.FromException<int>(new Exception("Test exception.")));

            bool failed = false;
            asyncOpHandle.OnFailed.Connect((ex) => failed = true);

            // Simulate an update loop that updates the async operation service.
            do {
                Thread.Sleep(30);
                asyncOpService.UpdateOperationHandles();
            } while (asyncOpHandle.IsRunning);

            Assert.Multiple(() =>
            {
                Assert.That(asyncOpHandle.IsCompleted, Is.False);
                Assert.That(asyncOpHandle.IsCanceled, Is.False);
                Assert.That(asyncOpHandle.IsFailed, Is.True);
                Assert.That(failed, Is.True);
                Assert.That(asyncOpHandle.Result, Is.EqualTo(0));
                Assert.That(AsyncOperationService.OperationHandles, Is.Empty);
            });
        }

        [Test, Order(12)]
        public void PassAsyncOpServiceDispose()
        {
            ServiceLocator.Collection.Clear();
            ServiceLocator.AddService<IAsyncOperationService, AsyncOperationService>();

            IAsyncOperationService asyncOpService = ServiceLocator.GetService<IAsyncOperationService>();
            IAsyncOperationHandle asyncOpHandle = asyncOpService.RunAsync((token) => Task.Delay(1000, token));

            asyncOpService.Dispose();

            Assert.Multiple(() =>
            {
                Assert.That(asyncOpHandle.IsCompleted,              Is.False);
                Assert.That(asyncOpHandle.IsCanceled,               Is.True);
                Assert.That(asyncOpHandle.IsFailed,                 Is.False);
                Assert.That(AsyncOperationService.OperationHandles, Is.Empty);
            });
        }
    }
}
