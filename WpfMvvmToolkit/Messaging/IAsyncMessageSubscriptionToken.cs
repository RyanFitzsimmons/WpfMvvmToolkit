using System;
using System.Threading.Tasks;

namespace WpfMvvmToolkit.Messaging;

public interface IAsyncMessageSubscriptionToken
{
    Guid Guid { get; }
    Type MessageType { get; }
    Task Invoke(object message);
}