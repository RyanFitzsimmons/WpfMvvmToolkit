using System;

namespace WpfMvvmToolkit.Messaging
{
    public interface IMessageService
    {
        /// <summary>
        /// Subscribing with a host allows the use of the UnsubscribeFromAll method.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="host"></param>
        /// <param name="messageDelegate"></param>
        void Subscribe<TMessage>(object host, Action<TMessage> messageDelegate) where TMessage : class;
        IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> messageDelegate) where TMessage : class;
        void UnsubscribeFromAll(object host);
        void Unsubscribe(IMessageSubscriptionToken token);
        void Send<TMessage>(TMessage message) where TMessage : class;
        TRequest Send<TMessage, TRequest>(TMessage message) where TMessage : RequestMessage<TRequest>;
    }
}
