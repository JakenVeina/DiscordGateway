using System.Threading;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sandbox
{
    public static class EntryPoint
    {
        public static void Main()
        {
            using var host = new HostBuilder()
                .ConfigureServices(services => services
                    .AddHostedService<DiscordGatewayClientService>()
                    .Configure<HostOptions>(options => options.ShutdownTimeout = Timeout.InfiniteTimeSpan))
                .UseConsoleLifetime()
                .Build();

            host.Run();
        }
    }
}
