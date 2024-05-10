using System;
using System.Collections.Generic;

namespace ADM87.GameUtilities.Messaging
{
    internal class MessagingService<T> : IMessagingService<T> where T : IMessage
    {
        /// <summary>
        /// The consumers that are registered to receive messages.
        /// </summary>
        private readonly HashSet<IMessageConsumer<T>> _consumers = new HashSet<IMessageConsumer<T>>();

        /// <inheritdoc/>
        public void AddConsumer(IMessageConsumer<T> consumer)
        {
            if (consumer == null)
                throw new ArgumentNullException(nameof(consumer));

            if (_consumers.Contains(consumer))
                throw new InvalidOperationException("Consumer is already registered.");

            _consumers.Add(consumer);
        }

        /// <inheritdoc/>
        public void RemoveConsumer(IMessageConsumer<T> consumer)
        {
            if (consumer == null)
                throw new ArgumentNullException(nameof(consumer));

            if (!_consumers.Contains(consumer))
                throw new InvalidOperationException("Consumer is not registered.");

            _consumers.Remove(consumer);
        }

        /// <inheritdoc/>
        public void Send(T message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            foreach (var consumer in _consumers)
                consumer.ConsumeMessage(message);
        }

        /// <inheritdoc/>
        public bool HasConsumer(IMessageConsumer<T> consumer)
        {
            if (consumer == null)
                throw new ArgumentNullException(nameof(consumer));

            return _consumers.Contains(consumer);
        }
    }
}
