using EasyNetQ;
using System;

namespace FibonacciNumbersApp.Queues
{
    class RabbitMQConsumer : IQueueConsumer
    {
        readonly string queueConnectionString;

        public RabbitMQConsumer(string queueConnectionString)
        {
            this.queueConnectionString = queueConnectionString;
        }

        public void Consume(string queueName, Action<string> onMessage)
        {
            var bus = RabbitHutch.CreateBus(queueConnectionString);
            var aBus = bus.Advanced;
            var queue = aBus.QueueDeclare(queueName, autoDelete: true, exclusive: true, durable: false);

            aBus.Consume<string>(
                queue,
                (message, messageReceivedInfo) =>
                {
                    onMessage(message.Body);
                }
            );
        }
    }
}
