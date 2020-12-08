using System.Numerics;

namespace FibonacciNumbersCalculatorLib
{
    public interface IFibonacciNumbersCalculator
    {
        BigInteger CalculateNext(BigInteger number);
    }
}
