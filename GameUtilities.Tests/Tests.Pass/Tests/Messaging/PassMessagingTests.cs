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
            Assert.That(ServiceLocator.HasService<IMessagingService<PassMessagingMocks.ITestMessage>>(), Is.True);

            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceLocator.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();
            Assert.Multiple(() =>
            {
                Assert.That(messagingService, Is.Not.Null);
                Assert.That(messagingService, Is.TypeOf<MessagingService<PassMessagingMocks.ITestMessage>>());
                
                messagingService = MessageRegistry.GetMessagingService<PassMessagingMocks.ITestMessage>();
                Assert.That(messagingService, Is.Not.Null);
            });
        }

        [Test, Order(1)]
        public void PassMessagingServiceRegistersConsumer()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceLocator.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();

            PassMessagingMocks.TestConsumer consumer = new PassMessagingMocks.TestConsumer();
            messagingService.AddConsumer(consumer);

            Assert.That(messagingService.HasConsumer(consumer), Is.True);
        }

        [Test, Order(2)]
        public void PassMessagingServiceUnregistersConsumer()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceLocator.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();

            PassMessagingMocks.TestConsumer consumer = new PassMessagingMocks.TestConsumer();
            messagingService.AddConsumer(consumer);
            messagingService.RemoveConsumer(consumer);

            Assert.That(messagingService.HasConsumer(consumer), Is.False);
        }

        [Test, Order(3)]
        public void PassMessagingServiceSendsMessage()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceLocator.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();

            PassMessagingMocks.TestConsumer consumer = new PassMessagingMocks.TestConsumer();
            messagingService.AddConsumer(consumer);

            PassMessagingMocks.ITestMessage message = new PassMessagingMocks.TestMessage("Test Message");
            messagingService.Send(message);

            Assert.Multiple(() =>
            {
                Assert.That(consumer.ReceivedMessage, Is.True);
                Assert.That(consumer.LastMessage, Is.EqualTo(message));
            });
        }

        [Test, Order(4)]
        public void PassMessagingServiceThrowsOnNullConsumer()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceLocator.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();

            Assert.Multiple(() =>
            {
                Assert.That(() => messagingService.AddConsumer(null), Throws.ArgumentNullException);
                Assert.That(() => messagingService.RemoveConsumer(null), Throws.ArgumentNullException);
                Assert.That(() => messagingService.HasConsumer(null), Throws.ArgumentNullException);
            });
        }

        [Test, Order(5)]
        public void PassMessagingServiceThrowsOnDuplicateConsumer()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceLocator.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();

            PassMessagingMocks.TestConsumer consumer = new PassMessagingMocks.TestConsumer();
            messagingService.AddConsumer(consumer);

            Assert.That(() => messagingService.AddConsumer(consumer), Throws.InvalidOperationException);
        }

        [Test, Order(6)]
        public void PassMessagingServiceThrowsOnUnregisteredConsumer()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceLocator.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();
            PassMessagingMocks.TestConsumer consumer = new PassMessagingMocks.TestConsumer();

            Assert.That(() => messagingService.RemoveConsumer(consumer), Throws.InvalidOperationException);
        }

        [Test, Order(7)]
        public void PassMessagingServiceThrowsOnNullMessage()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceLocator.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();
            Assert.That(() => messagingService.Send(null), Throws.ArgumentNullException);
        }

        [Test, Order(8)]
        public void PassMessagingServiceThrowsOnNullConsumerForHasConsumer()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceLocator.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();
            Assert.That(() => messagingService.HasConsumer(null), Throws.ArgumentNullException);
        }

        [Test, Order(9)]
        public void PassMessagingServiceThrowsOnNullMessageConsumerForRegisterConsumer()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceLocator.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();
            Assert.That(() => messagingService.AddConsumer(null), Throws.ArgumentNullException);
        }

        [Test, Order(10)]
        public void PassMessagingServiceThrowsOnNullMessageConsumerForUnregisterConsumer()
        {
            IMessagingService<PassMessagingMocks.ITestMessage> messagingService = ServiceLocator.GetService<IMessagingService<PassMessagingMocks.ITestMessage>>();
            Assert.That(() => messagingService.RemoveConsumer(null), Throws.ArgumentNullException);
        }
    }
}
