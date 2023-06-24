using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPgRPCReverseProxy.Options;

namespace TCPgRPCReverseProxy.ReversProxyTechniques
{
    public class ServerAssignment
    {
        public ServerOptions Server { get; set; } = default!;
        public int Assignments { get; set; }

        public bool IsHealthy { get; set; }

        public int Weight { get => Server.Weight; }

    }
}
