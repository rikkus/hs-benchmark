using System.Collections.Generic;

namespace HS.Benchmark.Playground
{
    public static class EnumerableUtils
    {
        public static void IterateToEnd<T>(this IEnumerable<T> enumerable)
        {
            var enumerator = enumerable.GetEnumerator();

            while (enumerator.MoveNext())
            {
                // Do nothing.
            }
        }
    }
}