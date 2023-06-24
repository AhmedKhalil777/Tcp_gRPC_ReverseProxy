namespace TCPgRPCReverseProxy.Exceptions;

public class CanNotWriteToStreamException : AggregateException
{
    public CanNotWriteToStreamException(string ip, int port) : base($"Stream write is closed for client {ip}:{port}.")
    {
        
    }

    public CanNotWriteToStreamException(Exception innerException ,string ip, int port) 
        : base($"Can not write to stream for client {ip}:{port}." +
        $"[{innerException.GetType().Name}] {innerException.Message}", innerException)
    {
        
    }
}
