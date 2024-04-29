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
        /// Subscribes to the signal.
        /// </summary>
        /// <param name="callback"></param>
        public void Subscribe(Action callback)
        {
            _callbacks -= callback;
            _callbacks += callback;
        }

        /// <summary>
        /// Unsubscribes from the signal.
        /// </summary>
        /// <param name="callback"></param>
        public void Unsubscribe(Action callback)
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
        /// <exception cref="InvocationLockViolationException"></exception>
        public void Invoke()
        {
            if (_lock != this)
                throw new InvocationLockViolationException();

            _callbacks?.Invoke();
        }

        /// <summary>
        /// Invokes the signal.
        /// </summary>
        /// <param name="lockObject"></param>
        /// <exception cref="InvocationLockViolationException"></exception>
        public void Invoke(object lockObject)
        {
            if (_lock != lockObject)
                throw new InvocationLockViolationException();

            _callbacks?.Invoke();
        }
    }
}
