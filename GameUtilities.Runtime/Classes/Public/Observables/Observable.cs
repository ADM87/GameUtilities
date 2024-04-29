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
        private bool    _alwaysEmits;

        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value) && !_alwaysEmits)
                    return;

                _value = value;
                Emit(this, value);
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
