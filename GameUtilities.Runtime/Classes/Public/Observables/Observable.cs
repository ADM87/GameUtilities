using System;
using ADM87.GameUtilities.Signals;

namespace ADM87.GameUtilities.Observables
{
    /// <summary>
    /// Represents a value that can be observed for changes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Observable<T> : Signal1<T>
    {
        /// <summary>
        /// The value of the observable.
        /// </summary>
        private T       _value;
        /// <summary>
        /// Whether or not the observable should always emit a signal when the value is set,
        /// regardless if the value has changed.
        /// </summary>
        private bool    _alwaysEmits;

        /// <summary>
        /// The value of the observable. Emits a signal when set.
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value) && !_alwaysEmits)
                    return;

                _value = value;
                Emit(_key, value);
            }
        }

        public Observable(T value)
            : base()
        {
            _value = value;
        }
        public Observable(T value, bool alwaysEmits)
            : base()
        {
            _value = value;
            _alwaysEmits = alwaysEmits;
        }
    }
}
