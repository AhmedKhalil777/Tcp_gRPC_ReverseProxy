using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPgRPCReverseProxy.Options
{
    public class ReverseProxyOptions
    {
        public const string ReverseProxy = "ReverseProxy";

        public int RecievingThreadPoolCount { get; set; } = 5;
        public int AcceptingClientsThreadPoolCount { get; set; } = 3;
        public TCPOptions TCPOptions { get; set; } = default!;
        public IEnumerable<ServerOptions> Servers { get; set; } = default!;
    }

    public class TCPOptions
    {
        public string IP { get; set; } = default!;
        public int Port { get; set; }
        public int ClientRecieveTimeout { get; set; }
        public int ClientSendTimeout { get; set; }

        public int ReadBufferSize { get; set; } = 2048;

    }

    public class ServerOptions
    {
        public string ServergRPCEndpoint { get; set; } = default!;
        public int Weight { get; set; }

    }
}
