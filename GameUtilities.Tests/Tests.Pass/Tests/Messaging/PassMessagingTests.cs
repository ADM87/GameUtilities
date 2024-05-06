using ADM87.GameUtilities.Messaging;
using ADM87.GameUtilities.Services;

namespace GameUtilities.Tests
{
    [TestFixture]
    public class PassMessagingTests
    {
        [Test, Order(0)]
        public void PassMessagingServiceExists()
        {
            MessageRegistry.RegisterMessagingService<PassMessagingMocks.ITestMessage>();
            Assert.That(ServiceProvider.HasService<IMessagingService<PassMessagingMocks.ITestMessage>>(), Is.True);

            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceProvider.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();
            Assert.Multiple(() =>
            {
                Assert.That(messagingService, Is.Not.Null);
                Assert.That(messagingService, Is.TypeOf<MessagingService<PassMessagingMocks.ITestMessage>>());
            });
        }

        [Test, Order(1)]
        public void PassMessagingServiceRegistersReceiver()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceProvider.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();

            PassMessagingMocks.TestReceiver receiver = new PassMessagingMocks.TestReceiver();
            messagingService.RegisterReceiver(receiver);

            Assert.That(messagingService.HasReceiver(receiver), Is.True);
        }

        [Test, Order(2)]
        public void PassMessagingServiceUnregistersReceiver()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceProvider.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();

            PassMessagingMocks.TestReceiver receiver = new PassMessagingMocks.TestReceiver();
            messagingService.RegisterReceiver(receiver);
            messagingService.UnregisterReceiver(receiver);

            Assert.That(messagingService.HasReceiver(receiver), Is.False);
        }

        [Test, Order(3)]
        public void PassMessagingServiceSendsMessage()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceProvider.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();

            PassMessagingMocks.TestReceiver receiver = new PassMessagingMocks.TestReceiver();
            messagingService.RegisterReceiver(receiver);

            PassMessagingMocks.ITestMessage message = new PassMessagingMocks.TestMessage("Test Message");
            messagingService.SendMessage(message);

            Assert.Multiple(() =>
            {
                Assert.That(receiver.ReceivedMessage, Is.True);
                Assert.That(receiver.LastMessage, Is.EqualTo(message));
            });
        }

        [Test, Order(4)]
        public void PassMessagingServiceThrowsOnNullReceiver()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceProvider.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();

            Assert.Multiple(() =>
            {
                Assert.That(() => messagingService.RegisterReceiver(null), Throws.ArgumentNullException);
                Assert.That(() => messagingService.UnregisterReceiver(null), Throws.ArgumentNullException);
                Assert.That(() => messagingService.HasReceiver(null), Throws.ArgumentNullException);
            });
        }

        [Test, Order(5)]
        public void PassMessagingServiceThrowsOnDuplicateReceiver()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceProvider.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();

            PassMessagingMocks.TestReceiver receiver = new PassMessagingMocks.TestReceiver();
            messagingService.RegisterReceiver(receiver);

            Assert.That(() => messagingService.RegisterReceiver(receiver), Throws.InvalidOperationException);
        }

        [Test, Order(6)]
        public void PassMessagingServiceThrowsOnUnregisteredReceiver()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceProvider.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();
            PassMessagingMocks.TestReceiver receiver = new PassMessagingMocks.TestReceiver();

            Assert.That(() => messagingService.UnregisterReceiver(receiver), Throws.InvalidOperationException);
        }

        [Test, Order(7)]
        public void PassMessagingServiceThrowsOnNullMessage()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceProvider.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();
            Assert.That(() => messagingService.SendMessage(null), Throws.ArgumentNullException);
        }

        [Test, Order(8)]
        public void PassMessagingServiceThrowsOnNullReceiverForHasReceiver()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceProvider.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();
            Assert.That(() => messagingService.HasReceiver(null), Throws.ArgumentNullException);
        }

        [Test, Order(9)]
        public void PassMessagingServiceThrowsOnNullMessageConsumerForRegisterReceiver()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceProvider.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();
            Assert.That(() => messagingService.RegisterReceiver(null), Throws.ArgumentNullException);
        }

        [Test, Order(10)]
        public void PassMessagingServiceThrowsOnNullMessageConsumerForUnregisterReceiver()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceProvider.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();
            Assert.That(() => messagingService.UnregisterReceiver(null), Throws.ArgumentNullException);
        }
    }
}
