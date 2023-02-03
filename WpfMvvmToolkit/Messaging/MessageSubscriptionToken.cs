using System;

namespace WpfMvvmToolkit.Messaging
{
    internal class MessageSubscriptionToken<TMessage> : IMessageSubscriptionToken
    {
        internal MessageSubscriptionToken(Action<TMessage> action)
        {
            ArgumentNullException.ThrowIfNull(action);

            Action = action;
        }

        public Guid Guid { get; } = Guid.NewGuid();
        public Type MessageType => typeof(TMessage);
        public Action<TMessage> Action { get; init; }

        public void Invoke(object message)
        {
            Action((TMessage)message);
        }
    }
}
