namespace ADM87.GameUtilities.Signals
{
    public abstract class SignalBase
    {
        protected object _lock;

        public SignalBase()
        {
            _lock = this;
        }
        public SignalBase(object lockObject)
        {
            _lock = lockObject;
        }
    }
}
