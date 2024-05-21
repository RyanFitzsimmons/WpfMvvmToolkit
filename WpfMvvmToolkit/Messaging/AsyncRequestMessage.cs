namespace WpfMvvmToolkit.Messaging;

public abstract class AsyncRequestMessage<T> : IAsyncMessage
{
    public abstract T Requested { get; set; }
}
