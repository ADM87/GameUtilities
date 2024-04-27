using System;
using System.Threading;
using System.Threading.Tasks;

namespace ADM87.GameUtilities.Async
{
    public interface IAsyncService
    {
        public delegate Task AsyncOperation(CancellationToken cancellationToken);
        public delegate void AsyncOperationCallback();
        public delegate void AsyncOperationExceptionCallback(Exception exception);

        AsyncOperationHandle RunAsync(
            AsyncOperation operation,
            AsyncOperationCallback completed = null,
            AsyncOperationCallback cancelled = null,
            AsyncOperationExceptionCallback faulted = null,
            bool startImmediately = true);
    }
}
