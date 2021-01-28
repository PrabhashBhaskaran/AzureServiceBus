using Prabhash.Platform.AzureServiceBus.Library.Core;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prabhash.Platform.AzureServiceBus.Library.Implementation
{
    public class ServiceBusQueueDecorator : IServiceBusQueueDecorator
    {
        private readonly IServiceBusDequeue _deQueuer;
        private readonly IServiceBusEnqueue _enQueuer;

        public ServiceBusQueueDecorator(string connection, string queue)
        {
            _deQueuer = new ServiceBusDequeue(connection, queue);
            _enQueuer = new ServiceBusEnqueue(connection, queue);

        }

        public async Task CompleteAsync(IEnumerable<BrokeredMessage> messages)
        {
            await _deQueuer.CompleteAsync(messages);
        }

        public async Task<BrokeredMessage> DequeueAsync()
        {
            return await _deQueuer.DequeueAsync();
        }

        public async Task<BrokeredMessage> DequeueAsync(TimeSpan timeout)
        {
            return await _deQueuer.DequeueAsync(timeout);
        }

        public async Task<IEnumerable<BrokeredMessage>> DequeueAsync(int messageCount, TimeSpan timeout)
        {
            return await _deQueuer.DequeueAsync(messageCount, timeout);
        }

        public async Task EnqueueAsync(BrokeredMessage message)
        {
            await _enQueuer.EnqueueAsync(message);
        }

        public async Task EnqueueAsync(List<BrokeredMessage> messages)
        {
            await _enQueuer.EnqueueAsync(messages);
        }

        public async Task<bool> HasMessages()
        {
            return await _deQueuer.HasMessages();
        }


    }
}

