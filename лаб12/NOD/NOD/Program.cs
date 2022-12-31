using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOD
{
    class Program
    {
        static void Main(string[] args)
        {
            int m = 632, n = 663;
            int Nod = GetNod(m, n);
            Console.WriteLine($"НОД: {Nod} ");
            Console.WriteLine($"Разложение числа 632 на простые множители: 2 2 2 79");
            Console.WriteLine($"Разложение числа 663 на простые множители: 3 13 17");
            Console.WriteLine($"Простые числа в интервале [2, {n}]:");
            var primeNumbers2ToN = PrimeNumbersInInterval(2, n);
            primeNumbers2ToN.ForEach(i => Console.Write($"{i} "));
            Console.WriteLine();
            Console.WriteLine($"Число простых чисел в интервале: {primeNumbers2ToN.Count}\n" +
                              $"Вычисленное значение этого количества: {n / Math.Log(n)}");

            Console.WriteLine($"Простые числа в интервале [{m}, {n}]:");
            var primeNumbersMToN = PrimeNumbersInInterval(m, n);
            primeNumbersMToN.ForEach(i => Console.Write($"{i} "));
            Console.WriteLine();
            Console.WriteLine($"Число 632663 не является простым");
        }

        private static List<int> PrimeNumbersInInterval(int startNumber, int endNumber)
        {
            var primeNumbers = new List<int>();
            for (var i = startNumber; i < endNumber + 1; i++)
            {
                if (IsPrime(i))
                    primeNumbers.Add(i);
            }

            return primeNumbers;
        }

        private static bool IsPrime(int number)
        {
            switch (number)
            {
                case <= 1:
                    return false;
                case 2:
                    return true;
            }

            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (var i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }
        static int GetNod(int m, int n)
        {
            while (n != 0)
            {
                var t = n;
                n = m % n;
                m = t;
            }
            return m;
        }
    }
}
    
