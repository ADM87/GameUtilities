namespace GameUtilities.Tests
{
    public static class PassAsyncMocks
    {
        public class AsyncTestException : Exception
        {
            public AsyncTestException() : base() { }
        }

        public static async Task SuccessAsyncOperation(CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);
        }

        public static async Task CancelledAsyncOperation(CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }

        public static async Task FaultedAsyncOperation(CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);
            throw new AsyncTestException();
        }
    }
}
