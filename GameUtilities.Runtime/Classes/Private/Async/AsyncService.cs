using System;
using System.Collections.Generic;
using ADM87.GameUtilities.ServiceProvider;

namespace ADM87.GameUtilities.Async
{
    using OperationHandles = Dictionary<Guid, AsyncOperationHandle>;

    [ServiceDefinition(typeof(IAsyncService), isSingleton: true)]
    internal class AsyncService : IAsyncService
    {
        private readonly OperationHandles _operationHandles = new OperationHandles();

        /// <inheritdoc/>
        public AsyncOperationHandle RunAsync(IAsyncService.AsyncOperation operation, bool startImmediately = true)
        {
            AsyncOperationHandle handle = new AsyncOperationHandle(operation);
            handle.Completed.Connect(() => RemoveHandle(handle.Id));
            handle.Cancelled.Connect(() => RemoveHandle(handle.Id));
            handle.Faulted.Connect(ex => RemoveHandle(handle.Id));
            _operationHandles.Add(handle.Id, handle);

            if (startImmediately)
                handle.Start();

            return handle;
        }

        private void RemoveHandle(Guid id)
        {
            if (_operationHandles.ContainsKey(id))
                _operationHandles.Remove(id);
        }
    }
}
