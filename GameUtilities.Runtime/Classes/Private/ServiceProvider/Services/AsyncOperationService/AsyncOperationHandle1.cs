using System;
using System.Threading;
using System.Threading.Tasks;
using ADM87.GameUtilities.Signals;

namespace ADM87.GameUtilities.Services
{
    internal class AsyncOperationHandle<T> : AsyncOperationHandleBase, IAsyncOperationHandle<T>
    {
        /// <summary> Operation to run. </summary>
        private Func<CancellationToken, Task<T>> _operation;

        /// <inheritdoc />
        public T Result => IsCompleted ? OperationTask.Result : default;
        /// <inheritdoc />
        public Signal<T> OnCompleted { get; private set; }
        /// <inheritdoc />
        public Task<T> OperationTask { get; private set; }

        /// <summary> Initializes a new instance of the <see cref="AsyncOperationHandle{T}"/> class. </summary>
        internal AsyncOperationHandle(Func<CancellationToken, Task<T>> operation)
            : base()
        {
            _operation = operation;

            OnCompleted = new Signal<T>(SignalLock);
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
                OnCompleted.Emit(SignalLock, OperationTask.Result);
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
        public Task<T> RunAsync()
        {
            if (!IsPending)
                return OperationTask;

            OperationTask = Task.Run(async () => {
                T result = await _operation(_cancellationTokenSource.Token);
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                return result;
            }, _cancellationTokenSource.Token);

            return OperationTask;
        }
    }
}
