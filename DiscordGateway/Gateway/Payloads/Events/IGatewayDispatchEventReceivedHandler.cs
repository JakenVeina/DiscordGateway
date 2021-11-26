using System.Threading;
using System.Threading.Tasks;

namespace DiscordGateway.Gateway.Payloads.Events
{
    public interface IGatewayDispatchEventReceivedHandler
    {
        Task OnDispatchEventReceived(
            object              @event,
            CancellationToken   cancellationToken);
    }
}
