using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciNumbersApp.Queues
{
    interface IQueueConsumer
    {
        void Consume(string queueName, Action<string> onMessage);
    }
}
