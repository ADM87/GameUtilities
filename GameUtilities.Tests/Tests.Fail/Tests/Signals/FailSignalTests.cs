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
        /// Tests that a signal cannot be emitted without the proper key
        /// </summary>
        [Test, Order(1)]
        public void FailUnlockedSignalEmit()
        {
            Signal signal = new Signal();
            Assert.Throws<EmissionLockViolationException>(() => signal.Emit(Guid.NewGuid()));
        }

        /// <summary>
        /// Tests that a locked signal cannot be emitted without the proper key
        /// </summary>
        [Test, Order(2)]
        public void FailLockedSignalEmit()
        {
            Guid signalKey = Guid.NewGuid();
            Signal signal = new Signal(signalKey);
            Assert.Throws<EmissionLockViolationException>(() => signal.Emit());
        }
    }
}
