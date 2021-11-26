using System.Threading;
using System.Threading.Tasks;

namespace System
{
    internal interface ISystemClock
    {
        DateTimeOffset Now { get; }

        Task SleepAsync(
            TimeSpan            duration,
            CancellationToken   cancellationToken);
    }

    internal class SystemClock
        : ISystemClock
    {
        public DateTimeOffset Now
            => DateTimeOffset.Now;

        public Task SleepAsync(
                TimeSpan            duration,
                CancellationToken   cancellationToken)
            => Task.Delay(duration, cancellationToken);
    }
}
