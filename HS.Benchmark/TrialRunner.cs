using System;
using System.Diagnostics;

namespace HS.Benchmark
{
    public delegate void BasicAction();

    internal class TrialRunner
    {
        public string Name { get; set; }
        public BasicAction Action { get; set; }
        public TimeSpan MaxTime { get; set; }
        public InvocationCompleteAction RunCompleteAction { get; set; }
        public InvocationCompleteAction BatchCompleteAction { get; set; }
        private static readonly TimeSpan MinimumBatchTime = TimeSpan.FromSeconds(1);

        public TrialRunner(string name, BasicAction action, TimeSpan maxTime, InvocationCompleteAction runCompleteAction, InvocationCompleteAction batchCompleteAction)
        {
            Name = name;
            Action = action;
            MaxTime = maxTime;
            RunCompleteAction = runCompleteAction;
            BatchCompleteAction = batchCompleteAction;
        }

        public InvocationResult Run(string name, BasicAction action, TimeSpan maxTime, InvocationCompleteAction runCompleteAction, InvocationCompleteAction batchCompleteAction)
        {
            return new TrialRunner(name, action, maxTime, runCompleteAction, batchCompleteAction).Run();
        }

        public InvocationResult Run()
        {
            var totalStopwatch = new Stopwatch();
            var minMillisecondsPerIteration = double.MaxValue;

            totalStopwatch.Start();

            while (totalStopwatch.Elapsed < MaxTime)
            {
                var batchMinMillisecondsPerInvocation = InvokeAsBatch(Action);

                BatchCompleteAction.Invoke(new InvocationResult(Name, batchMinMillisecondsPerInvocation));

                minMillisecondsPerIteration = Math.Min(batchMinMillisecondsPerInvocation, minMillisecondsPerIteration);
            }

            totalStopwatch.Stop();

            var result = new InvocationResult(Name, minMillisecondsPerIteration);

            RunCompleteAction.Invoke(result);

            return result;
        }

        private static double InvokeAsBatch(BasicAction action)
        {
            var stopwatch = new Stopwatch();
            long invocationCount = 0;

            stopwatch.Start();

            while (stopwatch.Elapsed < MinimumBatchTime)
            {
                action.Invoke();
                ++invocationCount;
            }

            stopwatch.Stop();

            return stopwatch.Elapsed.TotalMilliseconds / invocationCount;
        }
    }
}
