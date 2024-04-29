using ADM87.GameUtilities.Signals;

namespace GameUtilities.Tests
{
    /// <summary>
    /// Tests for the pass signal.
    /// </summary>
    [TestFixture]
    public class PassSignalTests
    {
        /// <summary>
        /// Tests that the signal can be emitted.
        /// </summary>
        [Test, Order(1)]
        public void PassSignalEmit()
        {
            Signal signal = new Signal();
            signal.Connect(() => Assert.Pass());
            signal.Emit();
        }

        /// <summary>
        /// Tests that the signal can be emitted with a parameter.
        /// </summary>
        [Test, Order(2)]
        public void PassSignalEmitWithParameter()
        {
            Signal1<int> signal = new Signal1<int>();
            signal.Connect(value => Assert.That(value, Is.EqualTo(10)));
            signal.Emit(10);
        }

        /// <summary>
        /// Tests that the signal can be emitted with a lock object.
        /// </summary>
        [Test, Order(3)]
        public void PassSignalLockEmit()
        {
            Signal signal = new Signal(lockObject: this);
            signal.Connect(() => Assert.Pass());
            signal.Emit(lockObject: this);
        }
    }
}
