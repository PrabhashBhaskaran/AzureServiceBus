using Prabhash.Platform.AzureServiceBus.Library.Core;
using Microsoft.ServiceBus;
using System.Threading.Tasks;
namespace Prabhash.Platform.AzureServiceBus.Library.Implementation
{

    public class ServiceBusManager : IServiceBusManager
    {
        private readonly string _connection;
        private readonly NamespaceManager namespaceManager;

        public ServiceBusManager(string connection)
        {
            this._connection = connection;
            this.namespaceManager = NamespaceManager.CreateFromConnectionString(this._connection);
        }

        public async Task AddQueue(string name)
        {
            await namespaceManager.CreateQueueAsync(name);
        }

        public async Task<IServiceBusQueueDecorator> GetDecoratedQueue(string name)
        {
            IServiceBusQueueDecorator decorator = null;
            if (await namespaceManager.QueueExistsAsync(name))
            {
                decorator = new ServiceBusQueueDecorator(_connection, name);
            }
            return decorator;
        }

        public async Task<bool> QueueExists(string name)
        {
            return await namespaceManager.QueueExistsAsync(name);
        }

        public async Task RemoveQueue(string name)
        {
            await namespaceManager.DeleteQueueAsync(name);
        }


    }
}

