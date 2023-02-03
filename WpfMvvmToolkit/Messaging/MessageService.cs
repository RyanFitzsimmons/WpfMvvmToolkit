using System;
using System.Collections.Generic;

namespace WpfMvvmToolkit.Messaging
{
    public class MessageService : IMessageService
    {
        private readonly Dictionary<Type, Dictionary<Guid, IMessageSubscriptionToken>> _subscriptions = new();

        public void Send<TMessage>(TMessage message) where TMessage : class
        {
            var messageType = typeof(TMessage);

            if (!_subscriptions.ContainsKey(messageType))
            {
                return;
            }

            foreach (var subscription in _subscriptions[messageType].Values)
            {
                subscription.Invoke(message);
            }
        }

        public IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> messageDelegate) where TMessage : class
        {
            MessageSubscriptionToken<TMessage> token = new(messageDelegate);

            if (!_subscriptions.ContainsKey(token.MessageType))
            {
                _subscriptions.Add(token.MessageType, new());
            }

            if (_subscriptions[token.MessageType].ContainsKey(token.Guid))
            {
                throw new Exception("Duplicate Guid!");
            }

            _subscriptions[token.MessageType].Add(token.Guid, token);

            return token;
        }

        public void Unsubscribe(IMessageSubscriptionToken token)
        {
            if (!_subscriptions.ContainsKey(token.MessageType) ||
                !_subscriptions[token.MessageType].ContainsKey(token.Guid))
            {
                return;
            }

            _subscriptions[token.MessageType].Remove(token.Guid);
        }
    }
}
