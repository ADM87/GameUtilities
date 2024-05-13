using System;
using System.Threading;
using System.Threading.Tasks;
using ADM87.GameUtilities.Signals;

namespace ADM87.GameUtilities.Services
{
    internal abstract class AsyncOperationHandleBase : IAsyncOperationHandleBase
    {
        /// <summary> Token source to cancel the operation. </summary>
        protected readonly CancellationTokenSource _cancellationTokenSource;

        /// <summary> Lock Id for signals. </summary>
        protected Guid SignalLock                { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public Guid Id                           { get; } = Guid.NewGuid();
        /// <inheritdoc />
        public Signal OnCanceled                 { get; private set; }
        /// <inheritdoc />
        public Signal<Exception> OnFailed       { get; private set; }

        /// <inheritdoc />
        public bool IsPending => GetTask() == null;
        /// <inheritdoc />
        public bool IsRunning
        {
            get
            {
                if (IsPending)
                    return false;

                Task task = GetTask();
                return !task.IsCompleted && !task.IsCanceled && !task.IsFaulted;
            }
        }
        /// <inheritdoc />
        public bool IsCompleted
        {
            get
            {
                if (IsPending)
                    return false;

                Task task = GetTask();
                return task.IsCompleted && !task.IsCanceled && !task.IsFaulted;
            }
        }
        /// <inheritdoc />
        public bool IsCanceled
        {
            get
            {
                if (IsPending)
                    return false;

                Task task = GetTask();
                return task.IsCanceled;
            }
        }
        /// <inheritdoc />
        public bool IsFailed
        {
            get
            {
                if (IsPending)
                    return false;

                Task task = GetTask();
                return task.IsFaulted;
            }
        }

        /// <summary> Creates a new instance of <see cref="AsyncOperationHandleBase"/>. </summary>
        internal AsyncOperationHandleBase()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            OnCanceled  = new Signal(SignalLock);
            OnFailed    = new Signal<Exception>(SignalLock);
        }

        /// <summary> Disposes the operation. </summary>
        internal virtual void Dispose()
        {
            _cancellationTokenSource.Dispose();
            CleanUp();
        }

        /// <summary> Cleans up the operation. </summary>
        internal virtual void CleanUp()
        {
            OnCanceled.Clear();
            OnFailed.Clear();
        }

        /// <summary> Gets the task of the operation. </summary>
        internal abstract Task GetTask();
        /// <summary> Finishes the operation. </summary>
        internal abstract void FinishOperation();

        /// <inheritdoc />
        public void Cancel()
        {
            if (_cancellationTokenSource.IsCancellationRequested || IsCompleted || IsCanceled || IsFailed)
                return;

            _cancellationTokenSource.Cancel();
        }
    }
}
