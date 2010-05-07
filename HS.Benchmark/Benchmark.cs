using System;

namespace HS.Benchmark
{
    public sealed class Benchmark : IDisposable
    {
        private readonly TimeSpan maxTime;

        private Benchmark(TimeSpan maxTime)
        {
            this.maxTime = maxTime;
        }

        public static Benchmark Begin(TimeSpan maxTime)
        {
            if (maxTime < TimeSpan.FromSeconds(1))
            {
                throw new ArgumentOutOfRangeException("maxTime", maxTime, "Must be one second or greater.");
            }

            return new Benchmark(maxTime);
        }

        public void Run(string name, BasicAction action)
        {
            new TrialRunner(name, action, maxTime, result => { }, result => { }).Run();
        }

        public void Run(string name, BasicAction action, InvocationCompleteAction runCompleteAction)
        {
            new TrialRunner(name, action, maxTime, runCompleteAction, result => { }).Run();
        }

        public void Run(string name, BasicAction action, InvocationCompleteAction runCompleteAction, InvocationCompleteAction batchCompleteAction)
        {
            new TrialRunner(name, action, maxTime, runCompleteAction, batchCompleteAction).Run();
        }

        public void Dispose()
        {
            // Nothing to do (yet).
        }
    }
}
