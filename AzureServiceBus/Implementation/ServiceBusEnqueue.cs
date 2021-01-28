namespace Prabhash.Platform.AzureServiceBus.Library.Implementation
{
    using Prabhash.Platform.AzureServiceBus.Library.Core;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public class ServiceBusEnqueue : IServiceBusEnqueue
    {
        private readonly string _connection;
        private readonly string _queue;
        private MessageSender messageSender;

        public ServiceBusEnqueue(string connection, string queue)
        {
            this._connection = connection;
            this._queue = queue;
            this.InitQueueSender();
        }

        private void InitQueueSender()
        {
            this.messageSender = MessagingFactory.CreateFromConnectionString(this._connection).CreateMessageSender(this._queue);
        }

        async Task IServiceBusEnqueue.EnqueueAsync(BrokeredMessage message)
        {
            await messageSender.SendAsync(message);
        }

        async Task IServiceBusEnqueue.EnqueueAsync(List<BrokeredMessage> messages)
        {
            await messageSender.SendBatchAsync(messages);
        }



    }
}

