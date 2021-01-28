namespace Prabhash.Platform.AzureServiceBus.Library.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;

    public interface IServiceBusDequeue
    {
        Task CompleteAsync(IEnumerable<BrokeredMessage> messages);
        Task<BrokeredMessage> DequeueAsync();
        Task<BrokeredMessage> DequeueAsync(TimeSpan timeout);
        Task<IEnumerable<BrokeredMessage>> DequeueAsync(int messageCount, TimeSpan timeout);
        Task<bool> HasMessages();
    }
}

