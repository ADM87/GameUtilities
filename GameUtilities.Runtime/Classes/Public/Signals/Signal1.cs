using System;

namespace ADM87.GameUtilities.Signals
{
    /// <summary>
    /// Represents a signal that can be invoked with one argument.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Signal1<T> : SignalBase
    {
        private event Action<T> _callbacks;

        public Signal1()
            : base() { }
        public Signal1(object lockObject)
            : base(lockObject) { }

        /// <summary>
        /// Subscribes to the signal.
        /// </summary>
        /// <param name="callback"></param>
        public void Subscribe(Action<T> callback)
        {
            _callbacks -= callback;
            _callbacks += callback;
        }

        /// <summary>
        /// Unsubscribes from the signal.
        /// </summary>
        /// <param name="callback"></param>
        public void Unsubscribe(Action<T> callback)
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
        /// <param name="arg"></param>
        /// <exception cref="InvocationLockViolationException"></exception>
        public void Invoke(T arg)
        {
            if (_lock != this)
                throw new InvocationLockViolationException();

            _callbacks?.Invoke(arg);
        }

        /// <summary>
        /// Invokes the signal.
        /// </summary>
        /// <param name="lockObject"></param>
        /// <param name="arg"></param>
        /// <exception cref="InvocationLockViolationException"></exception>
        public void Invoke(object lockObject, T arg)
        {
            if (_lock != lockObject)
                throw new InvocationLockViolationException();

            _callbacks?.Invoke(arg);
        }
    }
}
