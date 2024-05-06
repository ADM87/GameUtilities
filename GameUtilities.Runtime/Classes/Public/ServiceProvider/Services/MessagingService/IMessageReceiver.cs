namespace ADM87.GameUtilities.Messaging
{
    /// <summary>
    /// Represents a receiver of a specific message type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageConsumer<T> where T : IMessage
    {
        void ConsumeMessage(T message);
    }
}
