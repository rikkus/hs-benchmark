using System.Windows.Forms;

namespace HS.Benchmark.Playground
{
    class ListViewResultsCollector
    {
        public delegate void AddResultDelegate(InvocationResult result);

        public ListView ListView { get; private set; }

        public ListViewResultsCollector(ListView listView)
        {
            ListView = listView;
        }

        public void AddResult(InvocationResult result)
        {
            if (ListView.InvokeRequired)
                ListView.Invoke(new AddResultDelegate(AddResult), result);
            else
                ListView.Items.Add
                    (
                    new ListViewItem(
                        new []
                            {
                                result.AlgorithmName,
                                result.MinMillisecondsPerInvocation.ToString("0.00000000")
                            }
                        )
                    );

            Application.DoEvents();
        }
    }
}
