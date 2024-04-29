using System;
using System.Threading;
using System.Threading.Tasks;

namespace ADM87.GameUtilities.Async
{
    public interface IAsyncService
    {
        public delegate Task AsyncOperation(CancellationToken cancellationToken);

        AsyncOperationHandle RunAsync(AsyncOperation operation, bool startImmediately = true);
    }
}
