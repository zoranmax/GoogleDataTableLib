using System;
using System.Diagnostics;

namespace Google.DataTable.Net.Wrapper.Benchmark
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            for (int i = 1; i < 10; i++)
            {
                Console.WriteLine("Run nr: " + i);

                Run(i*100000);

                Console.WriteLine("");
            }

            Console.WriteLine("Click enter to exit...");
            Console.Read();
        }

        private static void Run(int nrOfRows)
        {
            var dt = new DataTable();
            var columnYear = new Column(ColumnType.Number, "Year");
            var columnCount = new Column(ColumnType.String, "Count");

            dt.AddColumn(columnYear);
            dt.AddColumn(columnCount);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < nrOfRows; i++)
            {
                var row = dt.NewRow();
                row.AddCellRange(new Cell[]
                    {
                        new Cell() {Value = 2012, Formatted = "2012"},
                        new Cell() {Value = 1, Formatted = "1"}
                    });

                dt.AddRow(row);
            }
            stopWatch.Stop();

            Console.WriteLine("Adding {0} rows takes {1} milliseconds ", nrOfRows, stopWatch.ElapsedMilliseconds);

            stopWatch.Restart();
            var json = dt.GetJson();
            stopWatch.Stop();
            Console.WriteLine("Serializing {0} rows takes {1} milliseconds ", nrOfRows,
                              stopWatch.ElapsedMilliseconds);
        }
    }
}