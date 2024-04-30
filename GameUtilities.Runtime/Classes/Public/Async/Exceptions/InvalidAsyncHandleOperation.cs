using System;

namespace ADM87.GameUtilities.Async
{
    /// <summary>
    /// Represents an exception that is thrown when an invalid operation is attempted on an <see cref="AsyncOperationHandle"/>.
    /// </summary>
    public sealed class InvalidAsyncHandleOperation : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="InvalidAsyncHandleOperation"/> class.
        /// </summary>
        public InvalidAsyncHandleOperation(string message = null)
            : base(message) {}
    }
}