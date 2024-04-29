using ADM87.GameUtilities.Observables;

namespace GameUtilities.Tests
{
    [TestFixture]
    public class PassObservableTests
    {
        [Test, Order(1)]
        public void PassObservableEmits()
        {
            Observable<int> observable = new Observable<int>(10, true);
            observable.Connect(value => Assert.Pass());
            observable.Value++;
        }

        [Test, Order(1)]
        public void PassObservableNoEmit()
        {
            Observable<int> observable = new Observable<int>(10, false);
            observable.Connect(value => Assert.Fail());
            observable.Value = 10;
        }

        [Test, Order(2)]
        public void PassObservableAlwaysEmitts()
        {
            int count = 0;
            Observable<int> observable = new Observable<int>(10, true);
            observable.Connect(value => count++);
            observable.Value = 10;
            Assert.That(count, Is.EqualTo(1));
        }
    }
}
