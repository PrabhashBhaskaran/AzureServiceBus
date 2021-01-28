using Microsoft.ServiceBus.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Prabhash.Platform.AzureServiceBus.Library.Core
{

    public interface IServiceBusEnqueue
    {
        Task EnqueueAsync(BrokeredMessage message);
        Task EnqueueAsync(List<BrokeredMessage> messages);
    }
}

