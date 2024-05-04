using System;
using System.Threading;
using System.Threading.Tasks;

namespace ADM87.GameUtilities.Services
{
    /// <summary>
    /// A service for running async operations.
    /// </summary>
    public interface IAsyncOperationService
    {
        /// <summary>
        /// Updates all operation handles.
        /// </summary>
        /// <remarks>
        /// This method should be called every frame to update the state of all operation handles.
        /// </remarks>
        void UpdateOperationHandles();
        /// <summary>
        /// Disposes of all operation handles and cancels any running operations.
        /// </summary>
        /// <remarks>
        /// This method will wait for all running operations to complete before disposing of them.
        /// </remarks>
        void Dispose();
        /// <summary>
        /// Run an async operation. Returna a handle to the operation that can be used to control it.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="startImmediately"></param>
        /// <returns></returns>
        IAsyncOperationHandle RunAsync(Func<CancellationToken, Task> operation, bool startImmediately = true);
        /// <summary>
        /// Run an async operation. Returna a handle to the operation that can be used to control it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation"></param>
        /// <param name="startImmediately"></param>
        /// <returns></returns>
        IAsyncOperationHandle<T> RunAsync<T>(Func<CancellationToken, Task<T>> operation, bool startImmediately = true);
    }
}
