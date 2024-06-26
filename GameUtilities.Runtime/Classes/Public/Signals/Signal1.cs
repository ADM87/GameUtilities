using System;

namespace ADM87.GameUtilities.Signals
{
    /// <summary>
    /// Represents a signal that can be invoked with one argument.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Signal<T> : SignalBase
    {
        /// <summary>
        /// The callbacks subscribed to the signal.
        /// </summary>
        private event Action<T> _callbacks;

        public Signal()            : base()    {}
        public Signal(Guid key)    : base(key) {}

        /// <summary>
        /// Connect to the signal.
        /// </summary>
        /// <param name="callback"></param>
        public void Connect(Action<T> callback)
        {
            _callbacks -= callback;
            _callbacks += callback;
        }

        /// <summary>
        /// Disconnect from the signal.
        /// </summary>
        /// <param name="callback"></param>
        public void Disconnect(Action<T> callback)
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
        /// <param name="arg"></param>
        /// <exception cref="EmissionLockViolationException"></exception>
        public void Emit(T arg)
        {
            if (IsLocked(Guid.Empty))
                throw new EmissionLockViolationException();

            _callbacks?.Invoke(arg);
        }

        /// <summary>
        /// Invokes the signal.
        /// </summary>
        /// <param name="lockObject"></param>
        /// <param name="arg"></param>
        /// <exception cref="EmissionLockViolationException"></exception>
        public void Emit(Guid key, T arg)
        {
            if (IsLocked(key))
                throw new EmissionLockViolationException();

            _callbacks?.Invoke(arg);
        }
    }
}
