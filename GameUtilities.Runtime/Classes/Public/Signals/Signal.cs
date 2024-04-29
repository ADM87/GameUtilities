using System;

namespace ADM87.GameUtilities.Signals
{
    /// <summary>
    /// Represents a signal that can be invoked without any arguments.
    /// </summary>
    public class Signal : SignalBase
    {
        private event Action  _callbacks;

        public Signal()
            : base() {}
        public Signal(object lockObject)
            : base(lockObject) {}

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
        public bool HasSubscribers()
        {
            return _callbacks != null;
        }

        /// <summary>
        /// Invokes the signal.
        /// </summary>
        /// <exception cref="EmissionLockViolationException"></exception>
        public void Emit()
        {
            if (_lock != this)
                throw new EmissionLockViolationException();

            _callbacks?.Invoke();
        }

        /// <summary>
        /// Invokes the signal.
        /// </summary>
        /// <param name="lockObject"></param>
        /// <exception cref="EmissionLockViolationException"></exception>
        public void Emit(object lockObject)
        {
            if (_lock != lockObject)
                throw new EmissionLockViolationException();

            _callbacks?.Invoke();
        }
    }
}
