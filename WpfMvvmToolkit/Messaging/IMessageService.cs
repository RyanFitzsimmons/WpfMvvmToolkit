using System;

namespace WpfMvvmToolkit.Messaging
{
    public interface IMessageService
    {
        IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> messageDelegate) where TMessage : class;
        void Unsubscribe(IMessageSubscriptionToken token);
        void Send<TMessage>(TMessage message) where TMessage : class;
    }
}
