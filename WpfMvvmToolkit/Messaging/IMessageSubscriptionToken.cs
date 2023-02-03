using System;

namespace WpfMvvmToolkit.Messaging
{
    public interface IMessageSubscriptionToken
    {
        Guid Guid { get; }
        Type MessageType { get; }
        void Invoke(object message);
    }
}
