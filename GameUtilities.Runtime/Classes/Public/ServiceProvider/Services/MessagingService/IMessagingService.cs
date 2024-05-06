namespace ADM87.GameUtilities.Messaging
{
    /// <summary>
    /// A service that allows for sending and receiving messages of a specific type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessagingService<T> where T : IMessage
    {
        /// <summary>
        /// Registers a receiver to receive messages.
        /// </summary>
        /// <param name="receiver"></param>
        void RegisterReceiver(IMessageConsumer<T> receiver);
        /// <summary>
        /// Unregisters a receiver from receiving messages.
        /// </summary>
        /// <param name="receiver"></param>
        void UnregisterReceiver(IMessageConsumer<T> receiver);
        /// <summary>
        /// Sends a message to all registered receivers.
        /// </summary>
        /// <param name="message"></param>
        void SendMessage(T message);
        /// <summary>
        /// Checks if a receiver is registered.
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns></returns>
        bool HasReceiver(IMessageConsumer<T> receiver);
    }
}
