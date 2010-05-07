using System.Xml.Linq;

namespace HS.Benchmark.Playground
{
    public class HtmlResultsCollector
    {
        private readonly XDocument resultsDocument = new XDocument();
        private readonly XElement tableElement;

        public XDocument Document
        {
            get
            {
                return resultsDocument;
            }
        }

        public HtmlResultsCollector()
        {
            resultsDocument.Add
                (
                new XElement("html",
                             new XElement("head",
                                          new XElement("title", "Benchmark results")
                                 ),
                             new XElement("body",
                                          tableElement = new XElement("table",
                                                                      new XElement
                                                                          (
                                                                          "tr",
                                                                          new XElement("th", "Name"),
                                                                          new XElement("th", "Milliseconds")
                                                                          )
                                                             )
                                 )
                    )
                );
        }

        public void AddResult(InvocationResult result)
        {
            tableElement.Add
                (
                new XElement
                    (
                    "tr",
                    new XElement("td", result.AlgorithmName),
                    new XElement("td", result.MinMillisecondsPerInvocation.ToString("0.00000000"))
                    )
                );
        }
    }
}