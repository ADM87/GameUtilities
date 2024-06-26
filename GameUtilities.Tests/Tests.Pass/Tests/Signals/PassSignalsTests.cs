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
            Signal<int> signal = new Signal<int>();
            signal.Connect(value => Assert.That(value, Is.EqualTo(10)));
            signal.Emit(10);
        }

        /// <summary>
        /// Tests that the signal can be emitted with a lock object.
        /// </summary>
        [Test, Order(3)]
        public void PassSignalLockEmit()
        {
            Guid signalKey = Guid.NewGuid();
            Signal signal = new Signal(signalKey);
            signal.Connect(() => Assert.Pass());
            signal.Emit(signalKey);
        }
    }
}
