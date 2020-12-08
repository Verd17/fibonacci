using FibonacciNumbersApp.Queues;
using FibonacciNumbersApp.RestClient;
using FibonacciNumbersCalculatorLib;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;

namespace FibonacciNumbersApp
{
    class Calculator
    {
        readonly IRestFibonacciClient restClient;
        readonly IQueueConsumer queueConsumer;
        readonly IFibonacciNumbersCalculator fibonacciCalculator;

        public Calculator(IRestFibonacciClient restClient, IQueueConsumer queueConsumer, IFibonacciNumbersCalculator fibonacciCalculator)
        {
            this.restClient = restClient;
            this.queueConsumer = queueConsumer;
            this.fibonacciCalculator = fibonacciCalculator;
        }

        public void RunCalculations(int count)
        {
            for (int i = 0; i < count; i++)
            {
                new Thread(StartCalculation).Start(i);
            }
        }

        async void StartCalculation(object obj)
        {
            if (!(obj is int))
                return;

            int id = (int)obj;

            string queueName = string.Format("{0}_{1}", Process.GetCurrentProcess().Id, id);

            queueConsumer.Consume(queueName, async (message) =>
                {
                    if (!BigInteger.TryParse(message, out BigInteger number))
                    {
                        Console.WriteLine("{0}: Received number is not an integer: {1}", id, message);
                        return;
                    }

                    Console.WriteLine("{0}: Received number {1}", id, message);

                    try
                    {
                        BigInteger nextNumber = fibonacciCalculator.CalculateNext(number);
                        try
                        {
                            Console.WriteLine("{0}: Sending number {1}", id, nextNumber);
                            var result = await restClient.SendNumberAsync(queueName, nextNumber);
                            if (!result.IsSuccess)
                            {
                                Console.WriteLine(result.Message);
                                return;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0}: Error posting result to service: {1}", id, e.Message);
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0}: Error calculating next Fibonacci number: {1}", id, e.Message);
                        return;
                    }
                }
            );

            //First request
            Console.WriteLine("{0}: Sending number {1}", id, 1);
            var firstResult = await restClient.SendNumberAsync(queueName, 1);
            if (!firstResult.IsSuccess)
            {
                Console.WriteLine(firstResult.Message);
                return;
            }
        }
    }
}
