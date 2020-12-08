using FibonacciNumbersCalculatorLib;
using FibonacciNumbersService.Models;
using FibonacciNumbersService.Queues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace FibonacciNumbersService.Controllers
{
    [Route("api/fibonacci-numbers-calculations")]
    [ApiController]
    public class FibonacciNumbersCalculationsController : ControllerBase
    {
        readonly IFibonacciNumbersCalculator calculator;
        readonly IQueuePublisher queuePublisher;

        public FibonacciNumbersCalculationsController(IFibonacciNumbersCalculator calculator, IQueuePublisher queuePublisher)
        {
            this.calculator = calculator;
            this.queuePublisher = queuePublisher;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CalculationRequest calculationRequest)
        {
            if (!BigInteger.TryParse(calculationRequest.Number, out BigInteger number))
                return UnprocessableEntity("'Number' request parameter is not an integer");

            try
            {
                BigInteger nextNumber = calculator.CalculateNext(number);

                try
                {
                    await queuePublisher.PublishAsync(calculationRequest.QueueName, nextNumber.ToString());
                    return Ok();
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error publishing result using queue: " + e.Message);
                }
            }
            catch (Exception e)
            {
                return UnprocessableEntity("Error calculating next Fibonacci number: " + e.Message);
            }
        }
    }
}
