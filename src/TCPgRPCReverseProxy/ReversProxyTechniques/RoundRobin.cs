using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPgRPCReverseProxy.Exceptions;
using TCPgRPCReverseProxy.Options;

namespace TCPgRPCReverseProxy.ReversProxyTechniques
{
    public class RoundRobin : IReverseProxyTechnique
    {
        private readonly ServerAssignment[] _servers;
        private int _currentIndex = 0;
        private int _currentAssignmentCount = 0;
        public RoundRobin(IEnumerable<ServerOptions> serverOptions)
        {
            _servers = serverOptions.OrderBy(x=> x.Weight)
                                    .Select(x => new ServerAssignment (x))
                                    .ToArray();
        }
        public string GetNextUpStream()
        {
            if (!_servers.All(x => x.IsHealthy))
                throw new UpStreamUnhealthyException();
            while (true)
            {
                if (_servers[_currentIndex].IsHealthy && _servers[_currentIndex].Weight > _currentAssignmentCount )
                {
                    break;
                }
                _currentAssignmentCount = 0;
                if (_currentIndex >= _servers.Length -1)
                {
                    _currentIndex = -1;
                }
                _currentIndex++;
            }
            var server = _servers[_currentIndex];
            _currentAssignmentCount++;
            return server.Server.ServergRPCEndpoint!;


        }
    }
}
