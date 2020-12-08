using FibonacciNumbersApp.Queues;
using FibonacciNumbersApp.RestClient;
using FibonacciNumbersCalculatorLib;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FibonacciNumbersApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out int count))
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                var configuration = builder.Build();

                IConfigurationSection appSettings = configuration.GetSection("AppSettings");

                Calculator calculator = new Calculator(
                    new RestFibonacciClient(appSettings["ServiceURL"]),
                    new RabbitMQConsumer(appSettings["QueueConnectionString"]),
                    new FibonacciNumbersCalculator());

                calculator.RunCalculations(count);
            }
        }
    }
}
