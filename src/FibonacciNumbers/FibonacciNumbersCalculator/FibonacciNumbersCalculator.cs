using System;
using System.Numerics;

namespace FibonacciNumbersCalculatorLib
{
    public class FibonacciNumbersCalculator : IFibonacciNumbersCalculator
    {
        public BigInteger CalculateNext(BigInteger number)
        {
            if (number == 0)
                return 1;

            BigInteger prev = 1, cur = 1, temp;
            
            while (cur < number)
            {
                temp = cur;
                cur = prev + cur;
                prev = temp;
            }
            
            if (cur > number)
                throw new ApplicationException("Input value is not fibonacci number");

            return prev + cur;
        }
    }
}
