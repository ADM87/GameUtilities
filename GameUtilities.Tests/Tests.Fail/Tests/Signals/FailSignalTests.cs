using ADM87.GameUtilities.Signals;

namespace GameUtilities.Tests
{
    /// <summary>
    /// Tests for the fail signal.
    /// </summary>
    [TestFixture]
    public class FailSignalTests
    {
        /// <summary>
        /// Tests that an unlocked signal cannot be emitted with a lock object.
        /// </summary>
        [Test, Order(1)]
        public void FailUnlockedSignalEmit()
        {
            Signal signal = new Signal();
            Assert.Throws<EmissionLockViolationException>(() => signal.Emit(lockObject: this));
        }

        /// <summary>
        /// Tests that the signal cannot be emitted without a lock object.
        /// </summary>
        [Test, Order(2)]
        public void FailLockedSignalEmit()
        {
            Signal signal = new Signal(this);
            Assert.Throws<EmissionLockViolationException>(() => signal.Emit());
        }
    }
}
