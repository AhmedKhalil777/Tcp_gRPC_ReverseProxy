using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPgRPCReverseProxy.Exceptions
{
    public class ClientDisconnectedException : Exception
    {
        public ClientDisconnectedException(string ip, int port) : base($"Client {ip}:{port} disconnected.")
        {
            
        }
    }
}
