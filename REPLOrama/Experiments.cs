using CsvHelper;
using System.IO;
using System.Linq;

namespace LINQPadFoo
{
    public static class Experiments
    {
        public static double[] ReadTotalSecondsExperiment(string experimentName) =>
            new CsvReader(ReadFile(experimentName))
            .GetRecords<Run>()
            .Select(x => x.TotalSeconds)
            .ToArray();
        
        static StreamReader ReadFile(string experimentName) =>
            File.OpenText(FilePath(experimentName));

        static string FilePath(string experimentName) =>
            $"C:\\experiments\\{experimentName}.csv";
    }

    internal class Run // whats the right name here?
    {
        public double TotalSeconds { get; set; }
    }

}
