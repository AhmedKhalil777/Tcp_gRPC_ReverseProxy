using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using TCPgRPCReverseProxy.Options;
using TCPgRPCReverseProxy.Scheduling;

namespace TCPgRPCReverseProxy
{
    public class ReverseProxy : BackgroundService
    {
        private TaskFactory _taskFactory;
        private readonly ReverseProxyOptions _options;
        private readonly ILogger<ReverseProxy> _logger;
        private CancellationToken _cancellationToken = CancellationToken.None;

        public ReverseProxy(ILogger<ReverseProxy> logger, IOptions<ReverseProxyOptions> options)
        {
            _options = options.Value;
            _logger = logger;
            var taskScheduler = new BlockingTaskScheduler(options?.Value?.RecievingThreadPoolCount ?? 5);
            _taskFactory = new TaskFactory(_cancellationToken ,TaskCreationOptions.LongRunning, TaskContinuationOptions.LongRunning, taskScheduler);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _cancellationToken = stoppingToken;
            var ipAddress = IPAddress.Parse(_options.TCPOptions.IP);
            var tcpListener = new TcpListener(ipAddress, _options.TCPOptions.Port);
            tcpListener.Start();
            while (!stoppingToken.IsCancellationRequested)
            {
                var client = await tcpListener.AcceptTcpClientAsync(_cancellationToken);
                
                _logger.LogInformation("Accepting new client at: {time}", DateTimeOffset.Now);
                _taskFactory?.StartNew(() => { }).ConfigureAwait(false);
            }
        }



    }
}