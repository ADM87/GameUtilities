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
        public AsyncOperationHandle RunAsync(
            IAsyncService.AsyncOperation operation,
            IAsyncService.AsyncOperationCallback completed = null,
            IAsyncService.AsyncOperationCallback cancelled = null,
            IAsyncService.AsyncOperationExceptionCallback faulted = null,
            bool startImmediately = true)
        {
            AsyncOperationHandle handle = new AsyncOperationHandle(operation);
            handle.SetCallbacks(
                BindCallback(completed, handle.Id),
                BindCallback(cancelled, handle.Id),
                BindCallback(faulted,   handle.Id)
            );
            _operationHandles.Add(handle.Id, handle);

            if (startImmediately)
                handle.Start();

            return handle;
        }

        private IAsyncService.AsyncOperationCallback BindCallback(
            IAsyncService.AsyncOperationCallback callback,
            Guid id)
        {
            return () =>
            {
                if (_operationHandles.TryGetValue(id, out AsyncOperationHandle handle))
                    _operationHandles.Remove(id);

                callback?.Invoke();
            };
        }

        private IAsyncService.AsyncOperationExceptionCallback BindCallback(
            IAsyncService.AsyncOperationExceptionCallback callback,
            Guid id)
        {
            return (Exception exception) =>
            {
                if (_operationHandles.TryGetValue(id, out AsyncOperationHandle handle))
                    _operationHandles.Remove(id);

                callback?.Invoke(exception);
            };
        }
    }
}
