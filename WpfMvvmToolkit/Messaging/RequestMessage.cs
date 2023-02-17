namespace WpfMvvmToolkit.Messaging
{
    public abstract class RequestMessage<T>
    {
        public abstract T Requested { get; set; }
    }
}
