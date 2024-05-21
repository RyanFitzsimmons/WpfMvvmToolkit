using System;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Messaging
{
    public interface IMessageService
    {
        /// <summary>
        /// Subscribing with a host allows the use of the UnsubscribeFromAll method.
        /// </summary>
        void Subscribe<TMessage>(object host, Action<TMessage> messageDelegate)
            where TMessage : class, IMessage;

        /// <summary>
        /// Subscribing with a host allows the use of the UnsubscribeFromAll method.
        /// </summary>
        void SubscribeAsync<TMessage>(object host, Func<TMessage, Task> messageDelegate)
            where TMessage : class, IAsyncMessage;

        IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> messageDelegate)
            where TMessage : class, IMessage;

        IAsyncMessageSubscriptionToken SubscribeAsync<TMessage>(Func<TMessage, Task> messageDelegate)
            where TMessage : class, IAsyncMessage;

        void UnsubscribeFromAll(object host);

        void Unsubscribe(IMessageSubscriptionToken token);

        void UnsubscribeAsync(IAsyncMessageSubscriptionToken token);

        void Send<TMessage>(TMessage message)
            where TMessage : class, IMessage;

        Task SendAsync<TMessage>(TMessage message)
            where TMessage : class, IAsyncMessage;

        TRequest Send<TMessage, TRequest>(TMessage message)
            where TMessage : RequestMessage<TRequest>, IMessage;

        Task<TRequest> SendAsync<TMessage, TRequest>(TMessage message)
            where TMessage : AsyncRequestMessage<TRequest>, IAsyncMessage;
    }
}
