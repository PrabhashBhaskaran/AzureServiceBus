using Prabhash.Platform.AzureServiceBus.Library.Core;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prabhash.Platform.AzureServiceBus.Library.Implementation
{


    public class ServiceBusDequeue : IServiceBusDequeue
    {
        private readonly string _connection;
        private readonly string _queue;
        private MessageReceiver messageReceiver;

        public ServiceBusDequeue(string connection, string queue)
        {
            this._connection = connection;
            this._queue = queue;
            this.InitQueueReceiver();
        }

        private void InitQueueReceiver()
        {
            this.messageReceiver = MessagingFactory.CreateFromConnectionString(this._connection).CreateMessageReceiver(this._queue);
        }

        async Task IServiceBusDequeue.CompleteAsync(IEnumerable<BrokeredMessage> messages)
        {
            List<Guid> tokens = new List<Guid>();
            foreach (var msg in messages)
            {
                tokens.Add(msg.LockToken);
            }

            await messageReceiver.CompleteBatchAsync(tokens);
        }

        async Task<BrokeredMessage> IServiceBusDequeue.DequeueAsync()
        {
            return await messageReceiver.ReceiveAsync();
        }

        async Task<BrokeredMessage> IServiceBusDequeue.DequeueAsync(TimeSpan timeout)
        {
            return await messageReceiver.ReceiveAsync(timeout);
        }

        async Task<IEnumerable<BrokeredMessage>> IServiceBusDequeue.DequeueAsync(int messageCount, TimeSpan timeout)
        {
            return await messageReceiver.ReceiveBatchAsync(messageCount, timeout);
        }

        async Task<bool> IServiceBusDequeue.HasMessages()
        {
           return await messageReceiver.PeekAsync()!=null;
        }


    }
}


