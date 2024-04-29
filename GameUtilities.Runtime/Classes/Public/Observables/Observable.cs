using ADM87.GameUtilities.Signals;

namespace ADM87.GameUtilities.Observables
{
    /// <summary>
    /// Represents a value that can be observed for changes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Observable<T> : Signal1<T>
    {
        private T       _value;
        private bool    _alwaysInvoke;

        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value) && !_alwaysInvoke)
                    return;

                _value = value;
                Invoke(this, value);
            }
        }

        public Observable(T value)
            : base()
        {
            _value = value;
        }
        public Observable(T value, bool alwaysInvoke)
            : base()
        {
            _value = value;
            _alwaysInvoke = alwaysInvoke;
        }
    }
}
