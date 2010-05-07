using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HS.Benchmark
{
    public sealed class BackgroundBenchmark : IDisposable
    {
        private readonly TimeSpan maxTime;
        private readonly InvocationCompleteAction finishedAction;
        private readonly Queue<TrialRunner> trialRunners = new Queue<TrialRunner>();
        private readonly BackgroundWorker worker = new BackgroundWorker();

        private InvocationResult bestInvocationResult = new InvocationResult(String.Empty, double.MaxValue);

        private BackgroundBenchmark(TimeSpan maxTime, InvocationCompleteAction finishedAction)
        {
            this.maxTime = maxTime;
            this.finishedAction = finishedAction;
            worker.DoWork += WorkerOnDoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (trialRunners.Count == 0)
            {
                finishedAction.Invoke(bestInvocationResult);
                return;
            }

            worker.RunWorkerAsync();
        }

        public static BackgroundBenchmark Create(TimeSpan maxTime)
        {
            return Create(maxTime, action => { });
        }

        public static BackgroundBenchmark Create(TimeSpan maxTime, InvocationCompleteAction finishedAction)
        {
            if (maxTime < TimeSpan.FromSeconds(1))
            {
                throw new ArgumentOutOfRangeException("maxTime", maxTime, "Must be one second or greater.");
            }

            return new BackgroundBenchmark(maxTime, finishedAction);
        }

        public void Begin()
        {
            worker.RunWorkerAsync();
        }

        private void WorkerOnDoWork(object sender, DoWorkEventArgs args)
        {
            trialRunners.Dequeue().Run();
        }

        public void Add(string name, BasicAction action)
        {
            trialRunners.Enqueue(new TrialRunner(name, action, maxTime, a => { }, a => { }));
        }

        public void Add(string name, BasicAction action, InvocationCompleteAction runCompleteAction)
        {
            trialRunners.Enqueue(new TrialRunner(name, action, maxTime, runCompleteAction, a => { }));
        }

        public void Add(string name, BasicAction action, InvocationCompleteAction runCompleteAction, InvocationCompleteAction batchCompleteAction)
        {
            trialRunners.Enqueue(new TrialRunner(name, action, maxTime, runCompleteAction, batchCompleteAction));
        }

        public void Dispose()
        {
            // Nothing to do (yet).
        }
    }
}
