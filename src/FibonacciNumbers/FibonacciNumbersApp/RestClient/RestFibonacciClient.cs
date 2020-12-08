using Newtonsoft.Json;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciNumbersApp.RestClient
{
    class RestFibonacciClient : IRestFibonacciClient
    {
        readonly string serviceURL;
        readonly HttpClient httpClient = new HttpClient();

        public RestFibonacciClient(string serviceURL)
        {
            this.serviceURL = serviceURL;
        }

        public async Task<Response> SendNumberAsync(string topic, BigInteger number)
        {
            var result = await httpClient.PostAsync(serviceURL, CreateContent(topic, number));
            if (!result.IsSuccessStatusCode)
            {
                return new Response { IsSuccess = false, Message = string.Format("{0}: Response StatusCode: {1}, ReasonPhrase: {2}", topic, result.StatusCode, result.ReasonPhrase) };
            }
            return new Response { IsSuccess = true };
        }

        private StringContent CreateContent(string topic, BigInteger number)
        {
            return new StringContent(JsonConvert.SerializeObject(new RequestData { QueueName = topic, Number = number.ToString() }), Encoding.UTF8, "application/json");
        }
    }
}
