using System.Numerics;
using System.Threading.Tasks;

namespace FibonacciNumbersApp.RestClient
{
    interface IRestFibonacciClient
    {
        Task<Response> SendNumberAsync(string topic, BigInteger number);
    }
}
