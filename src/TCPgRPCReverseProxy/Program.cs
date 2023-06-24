using TCPgRPCReverseProxy;
using TCPgRPCReverseProxy.Options;
using TCPgRPCReverseProxy.ReversProxyTechniques;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        var reverseProxyOptions = new ReverseProxyOptions();
        ctx.Configuration.GetSection(ReverseProxyOptions.ReverseProxy).Bind(reverseProxyOptions);
        services.AddSingleton(x => reverseProxyOptions.Servers);
        services.AddSingleton<IReverseProxyTechnique, RoundRobin>();
        services.Configure<ReverseProxyOptions>(ctx.Configuration.GetSection(ReverseProxyOptions.ReverseProxy));
        services.AddHostedService<ReverseProxy>();
    })
    .Build();

await host.RunAsync();
