using System;
using System.Collections.Generic;

namespace ADM87.GameUtilities.Messaging
{
    internal class MessagingService<T> : IMessagingService<T> where T : IMessage
    {
        /// <summary>
        /// The receivers that are registered to receive messages.
        /// </summary>
        private readonly HashSet<IMessageConsumer<T>> _receivers = new HashSet<IMessageConsumer<T>>();

        /// <inheritdoc/>
        public void RegisterReceiver(IMessageConsumer<T> receiver)
        {
            if (receiver == null)
                throw new ArgumentNullException(nameof(receiver));

            if (_receivers.Contains(receiver))
                throw new InvalidOperationException("Receiver is already registered.");

            _receivers.Add(receiver);
        }

        /// <inheritdoc/>
        public void UnregisterReceiver(IMessageConsumer<T> receiver)
        {
            if (receiver == null)
                throw new ArgumentNullException(nameof(receiver));

            if (!_receivers.Contains(receiver))
                throw new InvalidOperationException("Receiver is not registered.");

            _receivers.Remove(receiver);
        }

        /// <inheritdoc/>
        public void SendMessage(T message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            foreach (var receiver in _receivers)
                receiver.ConsumeMessage(message);
        }

        /// <inheritdoc/>
        public bool HasReceiver(IMessageConsumer<T> receiver)
        {
            if (receiver == null)
                throw new ArgumentNullException(nameof(receiver));

            return _receivers.Contains(receiver);
        }
    }
}
