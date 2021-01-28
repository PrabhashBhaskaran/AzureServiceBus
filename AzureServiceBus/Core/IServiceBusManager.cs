using System.Threading.Tasks;
namespace Prabhash.Platform.AzureServiceBus.Library.Core
{


    public interface IServiceBusManager
    {
        Task AddQueue(string name);
        Task<IServiceBusQueueDecorator> GetDecoratedQueue(string name);
        Task<bool> QueueExists(string name);
        Task RemoveQueue(string name);
    }
}

