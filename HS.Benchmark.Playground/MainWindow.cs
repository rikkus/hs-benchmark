using System;
using System.Windows.Forms;

namespace HS.Benchmark.Playground
{
    public partial class MainWindow : Form
    {
        private static readonly TimeSpan MaxTimePerTrial = TimeSpan.FromSeconds(5);
        private readonly HtmlResultsCollector summaryResultsCollector = new HtmlResultsCollector();
        private readonly ListViewResultsCollector intermediateResultsCollector;

        public MainWindow()
        {
            InitializeComponent();

            webBrowser.DocumentText = "<html><body><h1>Running benchmarks...</h1></body></html>";

            intermediateResultsCollector =  new ListViewResultsCollector(listView);

            var timer = new Timer();
            timer.Tick += TimerElapsed;
            timer.Interval = 500;
            timer.Start();
        }

        void TimerElapsed(object sender, EventArgs e)
        {
            ((Timer) sender).Stop();

            RunBenchmark();
        }

        private void RunBenchmark()
        {
            const int Max = 16;

            using (var benchmark = BackgroundBenchmark.Create(MaxTimePerTrial, FinishedAction))
            {
                benchmark.Add
                    (
                    "Recursive",
                    () => TimeWasters.RecursiveFibonacci(Max),
                    summaryResultsCollector.AddResult,
                    intermediateResultsCollector.AddResult
                    );

                benchmark.Add
                    (
                    "Iterative",
                    () => TimeWasters.IterativeFibonacci(Max).IterateToEnd(),
                    summaryResultsCollector.AddResult,
                    intermediateResultsCollector.AddResult
                    );

                benchmark.Begin();
            }
        }

        private void FinishedAction(InvocationResult result)
        {
            if (InvokeRequired)
                Invoke(new InvocationCompleteAction(FinishedAction), result);
            else
                webBrowser.DocumentText = summaryResultsCollector.Document.ToString();
        }
    }
}
