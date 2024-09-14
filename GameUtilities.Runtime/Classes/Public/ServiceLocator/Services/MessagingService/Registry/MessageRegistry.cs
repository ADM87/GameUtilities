using ADM87.GameUtilities.Services;

namespace ADM87.GameUtilities.Messaging
{
    public static class MessageRegistry
    {
        /// <summary>
        /// Registers an IMessagingService with the ServiceLocator.
        /// </summary>
        /// <remarks>
        /// <typeparamref name="T"/> must be an interface type that derives from IMessage.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="messagingServiceLifeTime"></param>
        /// <returns>An instance of the registered IMessagingService.</returns>
        public static IMessagingService<T> RegisterMessagingService<T>(EServiceLifeTime messagingServiceLifeTime = EServiceLifeTime.Transient) where T : IMessage
        {
            ServiceLocator.AddService<IMessagingService<T>, MessagingService<T>>(messagingServiceLifeTime);
            return GetMessagingService<T>();
        }
        /// <summary>
        /// Gets an IMessagingService from the ServiceLocator.
        /// </summary>
        /// <remarks>
        /// <typeparamref name="T"/> must be an interface type that derives from IMessage.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IMessagingService<T> GetMessagingService<T>() where T : IMessage
        {
            return ServiceLocator.GetService<IMessagingService<T>>();
        }
    }
}
