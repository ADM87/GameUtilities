using System;

namespace ADM87.GameUtilities.Signals
{
    /// <summary>
    /// Represents a signal that can be invoked without any arguments.
    /// </summary>
    public class Signal : SignalBase
    {
        private event Action  _callbacks;

        public Signal()         : base() {}
        public Signal(Guid key) : base(key) {}

        /// <summary>
        /// Connent to the signal.
        /// </summary>
        /// <param name="callback"></param>
        public void Connect(Action callback)
        {
            _callbacks -= callback;
            _callbacks += callback;
        }

        /// <summary>
        /// Disconnect from the signal.
        /// </summary>
        /// <param name="callback"></param>
        public void Disconnect(Action callback)
        {
            _callbacks -= callback;
        }

        /// <summary>
        /// Clears all subscribers from the signal.
        /// </summary>
        public void Clear()
        {
            _callbacks = null;
        }

        /// <summary>
        /// Checks if the signal has any subscribers.
        /// </summary>
        /// <returns></returns>
        public bool HasConnections()
        {
            return _callbacks != null;
        }

        /// <summary>
        /// Invokes the signal.
        /// </summary>
        /// <exception cref="EmissionLockViolationException"></exception>
        public void Emit()
        {
            if (IsLocked(Guid.Empty))
                throw new EmissionLockViolationException();

            _callbacks?.Invoke();
        }

        /// <summary>
        /// Invokes the signal.
        /// </summary>
        /// <param name="lockKey"></param>
        /// <exception cref="EmissionLockViolationException"></exception>
        public void Emit(Guid key)
        {
            if (IsLocked(key))
                throw new EmissionLockViolationException();

            _callbacks?.Invoke();
        }
    }
}
