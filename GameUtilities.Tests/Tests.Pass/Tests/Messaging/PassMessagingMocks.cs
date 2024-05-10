using ADM87.GameUtilities.Messaging;

namespace GameUtilities.Tests
{
    public static class PassMessagingMocks
    {
        public interface ITestMessage : IMessage
        {
            int Value { get; }
        }

        public class TestMessage : ITestMessage
        {
            public string Name { get; }
            public int Value { get; private set; }

            public TestMessage(string name)
            {
                Name = name;
            }
        }

        public class TestConsumer : IMessageConsumer<ITestMessage>
        {
            public ITestMessage LastMessage { get; private set; }
            public bool ReceivedMessage => LastMessage != null;

            public void ConsumeMessage(ITestMessage message)
            {
                LastMessage = message;
            }
        }
    }
}
