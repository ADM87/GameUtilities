using System;
using System.Threading;
using System.Threading.Tasks;
using ADM87.GameUtilities.Signals;

namespace ADM87.GameUtilities.Async
{
    public sealed class AsyncOperationHandle
    {
        private readonly IAsyncService.AsyncOperation   _operation;
        private readonly CancellationTokenSource        _cancellationTokenSource;

        public EAsyncOperationPhase Phase           { get; private set; }   = EAsyncOperationPhase.Pending;
        public Task                 OperationTask   { get; private set; }   = null;
        public Guid                 Id              { get; }                = Guid.NewGuid();
        public Signal               Completed       { get; }
        public Signal               Cancelled       { get; }
        public Signal1<Exception>   Faulted         { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="AsyncOperationHandle"/> class.
        /// </summary>
        /// <param name="operation"></param>
        internal AsyncOperationHandle(IAsyncService.AsyncOperation operation)
        {
            _operation                  = operation;
            _cancellationTokenSource    = new CancellationTokenSource();

            Completed                   = new Signal(this);
            Cancelled                   = new Signal(this);
            Faulted                     = new Signal1<Exception>(this);
        }

        /// <summary>
        /// Starts the operation.
        /// </summary>
        /// <remarks>
        /// If the operation is not in the pending phase, an <see cref="InvalidOperationException"/> is thrown.
        /// </remarks>
        /// <exception cref="InvalidOperationException"></exception>
        public void Start()
        {
            if (Phase != EAsyncOperationPhase.Pending)
                throw new InvalidOperationException("Cannot start an operation that is not in the pending phase.");

            OperationTask = Task.Run(async () =>
                {
                    try
                    {
                        await _operation(_cancellationTokenSource.Token);
                        _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                        Phase = EAsyncOperationPhase.Completed;
                        Completed.Invoke(this);
                    }
                    catch (OperationCanceledException)
                    {
                        Phase = EAsyncOperationPhase.Cancelled;
                        Cancelled.Invoke(this);
                    }
                    catch (Exception e)
                    {
                        Phase = EAsyncOperationPhase.Faulted;

                        if (Faulted.HasSubscribers())
                            Faulted.Invoke(this, e);
                        else
                            throw;
                    }
                    finally
                    {
                        CleanUp();
                    }
                },
            _cancellationTokenSource.Token);

            Phase = EAsyncOperationPhase.Running;
        }

        /// <summary>
        /// Cancels the operation.
        /// </summary>
        public void Cancel()
        {
            if (Phase != EAsyncOperationPhase.Running)
                return;

            _cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Cleans up the operation.
        /// </summary>
        private void CleanUp()
        {
            Completed.Clear();
            Cancelled.Clear();
            Faulted.Clear();
            _cancellationTokenSource.Dispose();
        }
    }
}
