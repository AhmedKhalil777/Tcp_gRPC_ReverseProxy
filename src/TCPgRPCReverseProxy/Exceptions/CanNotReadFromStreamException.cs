using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPgRPCReverseProxy.Exceptions
{
    public class CanNotReadFromStreamException : Exception
    {
        public CanNotReadFromStreamException(string ip, int port) : base($"The client {ip}:{port} is not enabling the read.")
        {
            
        }
    }
}
