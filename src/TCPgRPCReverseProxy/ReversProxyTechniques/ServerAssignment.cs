using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System.Reactive.Linq;
using TCPgRPCReverseProxy.Messages;
using TCPgRPCReverseProxy.Options;

namespace TCPgRPCReverseProxy.ReversProxyTechniques;

public class ServerAssignment
{
    private UpstreamDataReciever.UpstreamDataRecieverClient _gRPCClient;
    public ServerAssignment(ServerOptions server)
    {
        Server = server;
        var channel = GrpcChannel.ForAddress(server.ServergRPCEndpoint);
        _gRPCClient = new UpstreamDataReciever.UpstreamDataRecieverClient(channel);
        HealthCheck();
        Observable.Interval(TimeSpan.FromMinutes(1)).Subscribe(x => {
            HealthCheck();
        });
    }

    private void HealthCheck()
    {
        try
        {
            var healthStatus = _gRPCClient.Healthy(new Empty());
            IsHealthy = healthStatus.Healthy;
        }
        catch (Exception ex)
        {
            IsHealthy = false;
        }
    }
    public ServerOptions Server { get; set; } = default!;
    public int Assignments { get; set; } = 0;

    public bool IsHealthy { get; set; } = false;

    public int Weight { get => Server.Weight; }

}
