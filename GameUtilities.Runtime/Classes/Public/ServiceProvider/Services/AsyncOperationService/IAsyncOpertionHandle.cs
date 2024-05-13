using System;
using System.Threading.Tasks;
using ADM87.GameUtilities.Signals;

namespace ADM87.GameUtilities.Services
{
    /// <summary> Interface for async operation handles. </summary>
    public interface IAsyncOperationHandleBase
    {
        /// <summary> Unique identifier of the operation. </summary>
        Guid Id { get; }
        /// <summary> Emits when the operation is canceled. </summary>
        Signal OnCanceled { get; }
        /// <summary> Emits when the operation has failed. </summary>
        Signal<Exception> OnFailed { get; }
        /// <summary> Whether the operation has started. </summary>
        bool IsPending { get; }
        /// <summary> Whether the operation is running. </summary>
        bool IsRunning { get; }
        /// <summary> Whether the operation is completed. </summary>
        bool IsCompleted { get; }
        /// <summary> Whether the operation has been canceled. </summary>
        bool IsCanceled { get; }
        /// <summary> Whether the operation has failed. </summary>
        bool IsFailed { get; }

        /// <summary> Cancels the operation. </summary>
        void Cancel();
    }

    /// <summary> Interface for async operation handles without a result. </summary>
    public interface IAsyncOperationHandle : IAsyncOperationHandleBase
    {
        /// <summary> Emits when the operation is completed. </summary>
        Signal OnCompleted { get; }
        /// <summary> Task running the operation. </summary>
        Task OperationTask { get; }

        /// <summary> Runs the operation asynchronously. </summary>
        Task RunAsync();
    }

    /// <summary> Interface for async operation handles with a result. </summary>
    /// <typeparam name="T"> Type of the result. </typeparam>
    public interface IAsyncOperationHandle<T> : IAsyncOperationHandleBase
    {
        /// <summary> Result of the operation. </summary>
        T Result { get; }
        /// <summary> Emits when the operation is completed. </summary>
        Signal<T> OnCompleted { get; }
        /// <summary> Task running the operation. </summary>
        Task<T> OperationTask  { get; }

        /// <summary> Runs the operation asynchronously. </summary>
        Task<T> RunAsync();
    }
}
