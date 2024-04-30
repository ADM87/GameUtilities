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
        /// If the operation is not in the pending phase, an <see cref="InvalidAsyncHandleOperation"/> is thrown.
        /// </remarks>
        /// <exception cref="InvalidAsyncHandleOperation"></exception>
        public void Start()
        {
            if (Phase != EAsyncOperationPhase.Pending)
                throw new InvalidAsyncHandleOperation($"Cannot start an async operation that is not in a pending phase. Phase={Phase}");

            OperationTask = Task.Run(async () =>
                {
                    try
                    {
                        await _operation(_cancellationTokenSource.Token);
                        _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                        Phase = EAsyncOperationPhase.Completed;
                        Completed.Emit(this);
                    }
                    catch (OperationCanceledException)
                    {
                        Phase = EAsyncOperationPhase.Cancelled;
                        Cancelled.Emit(this);
                    }
                    catch (Exception e)
                    {
                        Phase = EAsyncOperationPhase.Faulted;

                        if (Faulted.HasSubscribers())
                            Faulted.Emit(this, e);
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
                throw new InvalidAsyncHandleOperation($"Cannot cancel an operation that is not in the Running phase. Phase={Phase}");

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
