using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPgRPCReverseProxy.Exceptions
{
    public class UpStreamUnhealthyException : AggregateException
    {
        public UpStreamUnhealthyException() : base($"All Upstream servers are not healthy, No clients will be assigned.")
        {
            
        }
    }
}
