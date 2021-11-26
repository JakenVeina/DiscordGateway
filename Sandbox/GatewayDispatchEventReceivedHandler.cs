using System;
using System.Threading;
using System.Threading.Tasks;

using DiscordGateway.Gateway.Payloads.Events;

namespace Sandbox
{
    public class GatewayDispatchEventReceivedHandler
        : IGatewayDispatchEventReceivedHandler
    {
        public Task OnDispatchEventReceived(
            object              @event,
            CancellationToken   cancellationToken)
        {
            Console.WriteLine($"Event Received: {@event.GetType().Name}");

            return Task.CompletedTask;
        }
    }
}
