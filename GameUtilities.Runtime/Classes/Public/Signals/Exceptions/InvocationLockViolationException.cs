namespace ADM87.GameUtilities.Signals
{
    /// <summary>
    /// Exception thrown when a signal is invoked from an object that is not the lock object.
    /// </summary>
    public sealed class InvocationLockViolationException : System.Exception
    {
        public InvocationLockViolationException()
            : base("Signal invocation is locked to another object.") { }
    }
}
