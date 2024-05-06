using ADM87.GameUtilities.Services;

namespace ADM87.GameUtilities.Messaging
{
    public static class MessageRegistry
    {
        /// <summary>
        /// Registers an IMessagingService with the ServiceProvider.
        /// </summary>
        /// <remarks>
        /// <typeparamref name="T"/> must be an interface type that derives from IMessage.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="messagingServiceLifeTime"></param>
        /// <returns>An instance of the registered IMessagingService.</returns>
        public static IMessagingService<T> RegisterMessagingService<T>(EServiceLifeTime messagingServiceLifeTime = EServiceLifeTime.Transient) where T : IMessage
        {
            if (!typeof(T).IsInterface)
                throw new System.ArgumentException($"Type {typeof(T).Name} is not an interface type that derives from IMessage.");

            ServiceProvider.AddService<IMessagingService<T>, MessagingService<T>>(messagingServiceLifeTime);
            return ServiceProvider.GetService<IMessagingService<T>>();
        }
    }
}
