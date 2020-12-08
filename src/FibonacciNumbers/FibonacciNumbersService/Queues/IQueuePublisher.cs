using System.Threading.Tasks;

namespace FibonacciNumbersService.Queues
{
    public interface IQueuePublisher
    {
        Task PublishAsync(string queueName, string message);
    }
}
