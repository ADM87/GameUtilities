using System;

namespace ADM87.GameUtilities.Signals
{
    /// <summary>
    /// Represents a signal that can be invoked without any arguments.
    /// </summary>
    public abstract class SignalBase
    {
        /// <summary>
        /// The key used to lock the signal.
        /// Only this key can invoke the signal.
        /// </summary>
        protected Guid _key;

        public SignalBase()         { _key = Guid.Empty; }
        public SignalBase(Guid key) { _key = key; }

        /// <summary>
        /// Checks is the signal is locked using the provided key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool IsLocked(Guid key)
        {
            return !key.Equals(_key);
        }
    }
}
