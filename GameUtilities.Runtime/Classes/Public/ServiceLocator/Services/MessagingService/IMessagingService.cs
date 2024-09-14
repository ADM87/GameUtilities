namespace ADM87.GameUtilities.Messaging
{
    /// <summary>
    /// A service that allows for sending and receiving messages of a specific type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessagingService<T> where T : IMessage
    {
        /// <summary>
        /// Adds a consumer to receive messages.
        /// </summary>
        /// <param name="consumer"></param>
        void AddConsumer(IMessageConsumer<T> consumer);
        /// <summary>
        /// Removes a consumer from receiving messages.
        /// </summary>
        /// <param name="consumer"></param>
        void RemoveConsumer(IMessageConsumer<T> consumer);
        /// <summary>
        /// Sends a message to all registered consumers.
        /// </summary>
        /// <param name="message"></param>
        void Send(T message);
        /// <summary>
        /// Checks if a consumer is registered.
        /// </summary>
        /// <param name="consumer"></param>
        /// <returns></returns>
        bool HasConsumer(IMessageConsumer<T> consumer);
    }
}
