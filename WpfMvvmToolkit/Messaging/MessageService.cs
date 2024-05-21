using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Messaging;

public sealed class MessageService : IMessageService
{
    private readonly Dictionary<object, List<IMessageSubscriptionToken>> _hostSubscriptions = new();
    private readonly Dictionary<object, List<IAsyncMessageSubscriptionToken>> _hostAsyncSubscriptions = new();
    private readonly Dictionary<Type, Dictionary<Guid, IMessageSubscriptionToken>> _subscriptions = new();
    private readonly Dictionary<Type, Dictionary<Guid, IAsyncMessageSubscriptionToken>> _asyncSubscriptions = new();

    private MessageService() { }

    public static MessageService Instance { get; } = new();

    public TRequest Send<TMessage, TRequest>(TMessage message)
        where TMessage : RequestMessage<TRequest>, IMessage
    {
        Send(message);
        return message.Requested;
    }

    public void Send<TMessage>(TMessage message)
        where TMessage : class, IMessage
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

    public async Task<TRequest> SendAsync<TMessage, TRequest>(TMessage message)
        where TMessage : AsyncRequestMessage<TRequest>, IAsyncMessage
    {
        await SendAsync(message);
        return message.Requested;
    }

    public async Task SendAsync<TMessage>(TMessage message)
        where TMessage : class, IAsyncMessage
    {
        var messageType = typeof(TMessage);

        if (!_asyncSubscriptions.ContainsKey(messageType))
        {
            return;
        }

        foreach (var subscription in _asyncSubscriptions[messageType].Values)
        {
            await subscription.Invoke(message);
        }
    }

    public void Subscribe<TMessage>(object host, Action<TMessage> messageDelegate)
        where TMessage : class, IMessage
    {
        if (!_hostSubscriptions.ContainsKey(host))
        {
            _hostSubscriptions.Add(host, new());
        }

        _hostSubscriptions[host].Add(Subscribe(messageDelegate));
    }

    public IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> messageDelegate)
        where TMessage : class, IMessage
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

    public void SubscribeAsync<TMessage>(object host, Func<TMessage, Task> messageDelegate)
        where TMessage : class, IAsyncMessage
    {
        if (!_hostAsyncSubscriptions.ContainsKey(host))
        {
            _hostAsyncSubscriptions.Add(host, new());
        }

        _hostAsyncSubscriptions[host].Add(SubscribeAsync(messageDelegate));
    }

    public IAsyncMessageSubscriptionToken SubscribeAsync<TMessage>(Func<TMessage, Task> messageDelegate)
        where TMessage : class, IAsyncMessage
    {
        AsyncMessageSubscriptionToken<TMessage> token = new(messageDelegate);

        if (!_asyncSubscriptions.ContainsKey(token.MessageType))
        {
            _asyncSubscriptions.Add(token.MessageType, new());
        }

        if (_asyncSubscriptions[token.MessageType].ContainsKey(token.Guid))
        {
            throw new Exception("Duplicate Guid!");
        }

        _asyncSubscriptions[token.MessageType].Add(token.Guid, token);

        return token;
    }

    public void UnsubscribeFromAll(object host)
    {
        if (_hostSubscriptions.TryGetValue(host, out var subscription))
        {
            foreach (var token in subscription)
            {
                Unsubscribe(token);
            }

            _hostSubscriptions.Remove(host);
        }

        if (_hostAsyncSubscriptions.TryGetValue(host, out var asyncSubscription))
        {
            foreach (var token in asyncSubscription)
            {
                UnsubscribeAsync(token);
            }

            _hostAsyncSubscriptions.Remove(host);
        }
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

    public void UnsubscribeAsync(IAsyncMessageSubscriptionToken token)
    {
        if (!_asyncSubscriptions.ContainsKey(token.MessageType) ||
            !_asyncSubscriptions[token.MessageType].ContainsKey(token.Guid))
        {
            return;
        }

        _asyncSubscriptions[token.MessageType].Remove(token.Guid);
    }
}
