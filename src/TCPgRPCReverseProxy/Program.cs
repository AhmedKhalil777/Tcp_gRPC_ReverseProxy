using TCPgRPCReverseProxy;
using TCPgRPCReverseProxy.Options;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<ReverseProxyOptions>(ctx.Configuration.GetSection(ReverseProxyOptions.ReverseProxy));
        services.AddHostedService<ReverseProxy>();
    })
    .Build();

await host.RunAsync();
