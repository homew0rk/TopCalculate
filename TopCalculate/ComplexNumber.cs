using System.Numerics;
using System;

namespace TopCalculate
{
    public static class ComplexNumberOperations
    {
        public static Complex Add(Complex a, Complex b) => a + b;
        public static Complex Subtract(Complex a, Complex b) => a - b;
        public static Complex Multiply(Complex a, Complex b) => a * b;
        public static Complex Divide(Complex a, Complex b)
        {
            if (b == Complex.Zero) throw new DivideByZeroException("Деление на ноль невозможно.");
            return a / b;
        }
    }
}
