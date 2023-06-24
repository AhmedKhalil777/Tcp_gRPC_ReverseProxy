using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPgRPCReverseProxy.Exceptions;

namespace TCPgRPCReverseProxy
{
    public class TcpClinetReverseHandler : IDisposable
    {

        public EventHandler OnDataSent { get; set; }
        public EventHandler OnDataRecieved { get; set; }
        public EventHandler OnClientDisposed { get; set; }
        public EventHandler OnGRPCSent { get; set; }
        public EventHandler OnGRPCRecieve { get; set; }
        public EventHandler OnGRPCDisconnected { get; set; }



        public TcpClinetReverseHandler(TcpClient client, string gRPCEndpoint, int maxCountOfEmptyPackets)
        {
            Client = client;
            GRPCEndpoint = gRPCEndpoint;
            var remoteEndPoint = (IPEndPoint)client.Client.RemoteEndPoint!;
            IP = remoteEndPoint.Address.ToString();
            Port = remoteEndPoint.Port;
            MaxCountOfEmotyPackets = maxCountOfEmptyPackets;
        }


        private NetworkStream? _stream;
        public int MaxCountOfEmotyPackets { get; set; }
        public TcpClient Client { get; set; }

        public bool Connected { get => Client.Connected; }

        public int EmptyConsecutivePacketsCount { get; set; }
        public string IP { get; init; }
        public int Port { get; init; }
        public string GRPCEndpoint { get; set; } = default!;

        public NetworkStream Stream
        {
            get
            {
                if (_stream != null)
                    return _stream;
                _stream = Client.GetStream();
                return _stream;
            }
        }

        private async ValueTask ForwardTogRPCClient(byte[] data, CancellationToken cancellationToken)
        {

        }

        public async ValueTask ReverseProxyFromDownStreamToUpstream(CancellationToken cancellationToken)
        {
            var data = new byte[2048];
            while (true)
            {
                var isRead = await ReadAsync(data, cancellationToken);
                if (isRead)
                {
                    await ForwardTogRPCClient(data, cancellationToken);
                }
            }
        }


        public async ValueTask<bool> ReadAsync(byte[] data, CancellationToken cancellationToken)
        {
            HandleDisconnection();

            if (Stream.CanRead)
            {
                var readCount = await Stream.ReadAsync(data, cancellationToken);
                if (readCount == 0)
                {
                    EmptyConsecutivePacketsCount++;
                    return false;
                }
                OnDataRecieved.Invoke(this, new EventArgs { });

            }
            return false;

        }
        public async ValueTask WriteAsync(byte[] bytes, CancellationToken cancellationToken)
        {

            HandleDisconnection();
            if (Stream.CanWrite)
            {
                try
                {
                    await Stream.WriteAsync(bytes, cancellationToken);
                    OnDataSent.Invoke(this, new EventArgs { });

                }
                catch (Exception ex)
                {

                    throw new CanNotWriteToStreamException(ex, IP, Port);
                }
            }

            throw new CanNotWriteToStreamException(IP, Port);

        }


        public void HandleDisconnection()
        {
            if (Connected)
            {
                return;
            }

            Dispose();

            throw new ClientDisconnectedException(IP, Port);
            
        }
        public void Dispose()
        {
            OnClientDisposed.Invoke(this, new EventArgs { });
            Client?.Close();
            Client?.Dispose();
        }
    }
}
