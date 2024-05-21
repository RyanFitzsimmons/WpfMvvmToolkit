using System;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Messaging;

internal class AsyncMessageSubscriptionToken<TMessage> : IAsyncMessageSubscriptionToken
{
    public AsyncMessageSubscriptionToken(Func<TMessage, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        
        Action = action;
    }

    public Guid Guid { get; } = Guid.NewGuid();
    public Type MessageType => typeof(TMessage);
    public Func<TMessage, Task> Action { get; init; }
    
    public async Task Invoke(object message)
    {
        await Action.Invoke((TMessage)message);
    }
}