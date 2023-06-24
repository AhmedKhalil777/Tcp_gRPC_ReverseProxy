using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Text;
using TCPgRPCReverseProxy.Messages;

namespace GRPCService.Services
{
    public class UpstreamDataService : UpstreamDataReciever.UpstreamDataRecieverBase
    {
        private readonly ILogger<UpstreamDataService> _logger;
        public UpstreamDataService(ILogger<UpstreamDataService> logger)
        {
            _logger = logger;
        }

        public override Task<UpstreamDataResponse> RecieveData(UpstreamDataRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"[{DateTime.Now}] Client {request.Ip}:{request.Port} : {Encoding.UTF8.GetString(request.RawData.Span)}");
            return Task.FromResult(new UpstreamDataResponse {  Recieved = true });
        }

        public override Task<HealthResponse> Healthy(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new HealthResponse { Healthy = true});
        }
    }
}
