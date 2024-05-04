using System;
using System.Threading;
using System.Threading.Tasks;
using ADM87.GameUtilities.Signals;

namespace ADM87.GameUtilities.Services
{
    internal class AsyncOperationHandle : AsyncOperationHandleBase, IAsyncOperationHandle
    {
        /// <summary> Operation to run. </summary>
        private Func<CancellationToken, Task> _operation;

        /// <inheritdoc />
        public Signal OnCompleted { get; private set; }
        /// <inheritdoc />
        public Task OperationTask { get; private set; }

        /// <summary> Initializes a new instance of the <see cref="AsyncOperationHandle"/> class. </summary>
        internal AsyncOperationHandle(Func<CancellationToken, Task> operation)
            : base()
        {
            _operation = operation;

            OnCompleted = new Signal(SignalLock);
        }

        /// <inheritdoc />
        internal override void FinishOperation()
        {
            if (IsCanceled)
            {
                OnCanceled.Emit(SignalLock);
            }
            else if (IsFailed)
            {
                if (OnFailed.HasConnections())
                    OnFailed.Emit(SignalLock, OperationTask.Exception);
                else
                    throw new Exception("Async operation failed.", OperationTask.Exception);
            }
            else
            {
                OnCompleted.Emit(SignalLock);
            }

            CleanUp();
        }

        /// <inheritdoc />
        internal override Task GetTask()
        {
            return OperationTask;
        }

        /// <inheritdoc />
        internal override void CleanUp()
        {
            OnCompleted.Clear();
            base.CleanUp();
        }

        /// <inheritdoc />
        internal override void Dispose()
        {
            _operation = null;
            base.Dispose();
        }

        /// <inheritdoc />
        public Task RunAsync()
        {
            if (!IsPending)
                return OperationTask;

            OperationTask = Task.Run(async () => {
                await _operation(_cancellationTokenSource.Token);
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
            }, _cancellationTokenSource.Token);

            return OperationTask;
        }
    }
}
