namespace HS.Benchmark
{
    public class InvocationResult
    {
        public string AlgorithmName { get; set; }
        public double MinMillisecondsPerInvocation { get; set; }

        public InvocationResult(string algorithmName, double minMillisecondsPerInvocation)
        {
            AlgorithmName = algorithmName;
            MinMillisecondsPerInvocation = minMillisecondsPerInvocation;
        }
    }
}
