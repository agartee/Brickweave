using System.Threading;
using System.Threading.Tasks;

namespace BasicMessaging.WebApp.HostedServices
{
    internal interface IMessagingAdapter
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
