using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ADM87.GameUtilities.Services
{
    using AsyncOperationHandles = Dictionary<Guid, AsyncOperationHandleBase>;

    [ServiceDefinition(typeof(IAsyncOperationService), EServiceLifeTime.Singleton)]
    internal class AsyncOperationService : IAsyncOperationService
    {
        /// <summary> A record of active operation handles. </summary>
        internal static readonly AsyncOperationHandles OperationHandles = new AsyncOperationHandles();

        /// <inheritdoc />
        public void UpdateOperationHandles()
        {
            List<AsyncOperationHandleBase> finsihedHandles = new List<AsyncOperationHandleBase>();

            foreach (AsyncOperationHandleBase handle in OperationHandles.Values)
            {
                if (handle.IsCompleted || handle.IsCanceled || handle.IsFailed)
                    finsihedHandles.Add(handle);
            }

            foreach (AsyncOperationHandleBase handle in finsihedHandles)
            {
                handle.FinishOperation();
                handle.Dispose();

                OperationHandles.Remove(handle.Id);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            List<Task> runningTask = new List<Task>();

            foreach (AsyncOperationHandleBase handle in OperationHandles.Values)
            {
                if (handle.IsRunning)
                    runningTask.Add(handle.GetTask());

                handle.Cancel();
                handle.Dispose();
            }

            OperationHandles.Clear();
        }

        /// <inheritdoc />
        public IAsyncOperationHandle RunAsync(Func<CancellationToken, Task> operation, bool startImmediately = true)
        {
            AsyncOperationHandle handle = new AsyncOperationHandle(operation);

            OperationHandles.Add(handle.Id, handle);

            if (startImmediately)
                handle.RunAsync();

            return handle;
        }

        /// <inheritdoc />
        public IAsyncOperationHandle<T> RunAsync<T>(Func<CancellationToken, Task<T>> operation, bool startImmediately = true)
        {
            AsyncOperationHandle<T> handle = new AsyncOperationHandle<T>(operation);

            OperationHandles.Add(handle.Id, handle);

            if (startImmediately)
                handle.RunAsync();

            return handle;
        }
    }
}