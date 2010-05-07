using System.Collections.Generic;

namespace HS.Benchmark.Playground
{
    class TimeWasters
    {
        public static int RecursiveFibonacci(int n)
        {
            if (n <= 1)
                return 1;

            return RecursiveFibonacci(n - 1) + RecursiveFibonacci(n - 2);
        }

        public static IEnumerable<int> IterativeFibonacci(int max)
        {
            int n1 = 0;
            int n2 = 1;

            yield return 1;

            if (max == 1)
                yield break;

            for (int i = 2; i < max; i++)
            {
                int n = n1 + n2;

                n1 = n2;
                n2 = n;

                yield return n;
            }
        }
    }
}