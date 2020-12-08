using EasyNetQ;
using EasyNetQ.Topology;
using System.Threading.Tasks;

namespace FibonacciNumbersService.Queues
{
    class RabbitMQPublisher : IQueuePublisher
    {
        readonly string connectionString;

        public RabbitMQPublisher(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task PublishAsync(string queueName, string message)
        {
            using (var bus = RabbitHutch.CreateBus(connectionString))
            {
                await bus.Advanced.PublishAsync(Exchange.GetDefault(), queueName, false, new Message<string>(message));
            }
        }
    }
}
