using System;
using System.Threading;
using System.Threading.Tasks;

namespace ADM87.GameUtilities.Async
{
    public sealed class AsyncOperationHandle
    {
        private readonly IAsyncService.AsyncOperation           _operation;
        private readonly CancellationTokenSource                _cancellationTokenSource;
        private IAsyncService.AsyncOperationCallback            _completed;
        private IAsyncService.AsyncOperationCallback            _cancelled;
        private IAsyncService.AsyncOperationExceptionCallback   _faulted;

        public EAsyncOperationPhase Phase   { get; private set; }   = EAsyncOperationPhase.Pending;
        public Guid Id                      { get; }                = Guid.NewGuid();
        public Task OperationTask           { get; private set; }   = null;

        internal AsyncOperationHandle(IAsyncService.AsyncOperation operation)
        {
            _operation                  = operation;
            _cancellationTokenSource    = new CancellationTokenSource();
        }

        internal void SetCallbacks(IAsyncService.AsyncOperationCallback completed,
                                   IAsyncService.AsyncOperationCallback cancelled,
                                   IAsyncService.AsyncOperationExceptionCallback faulted)
        {
            _completed  = completed;
            _cancelled  = cancelled;
            _faulted    = faulted;
        }

        public void Start()
        {
            if (Phase != EAsyncOperationPhase.Pending)
                throw new InvalidOperationException("Cannot start an operation that is not in the pending phase.");

            OperationTask = Task.Run(async () =>
                {
                    try
                    {
                        await _operation(_cancellationTokenSource.Token);
                        Phase = EAsyncOperationPhase.Completed;
                        _completed?.Invoke();
                    }
                    catch (OperationCanceledException)
                    {
                        Phase = EAsyncOperationPhase.Cancelled;
                        _cancelled?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Phase = EAsyncOperationPhase.Faulted;

                        if (_faulted != null)
                            _faulted(e);
                        else
                            throw;
                    }
                },
            _cancellationTokenSource.Token);

            Phase = EAsyncOperationPhase.Running;
        }

        public void Cancel()
        {
            if (Phase != EAsyncOperationPhase.Running)
                return;

            _cancellationTokenSource.Cancel();
        }
    }
}
