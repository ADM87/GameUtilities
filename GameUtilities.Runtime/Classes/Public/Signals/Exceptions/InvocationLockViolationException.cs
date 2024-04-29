namespace ADM87.GameUtilities.Signals
{
    /// <summary>
    /// Exception thrown when an object tries to emit a signal that is locked to another object.
    /// </summary>
    public sealed class EmissionLockViolationException : System.Exception
    {
        public EmissionLockViolationException()
            : base("Signal emission is locked to another object.") { }
    }
}
