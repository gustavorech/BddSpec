using System;
using BddSpec.Execution;

namespace BddSpec.Printer
{
    public class ExecutionMetricsPrinter
    {
        public static void Print(ExecutionMetrics executionMetrics)
        {
            Console.WriteLine();
            ConsolePrinter.WriteInfoLine("Execution metrics:");
            ConsolePrinter.WriteInfoLine("Test classes executed: " + executionMetrics.TotalTestClasses, 1);
            ConsolePrinter.WriteInfoLine("Nodes reached: " + executionMetrics.TotalNodesReached, 1);
            ConsolePrinter.WriteInfoLine("Leaf nodes reached: " + executionMetrics.TotalLeafNodes, 1);
            ConsolePrinter.WriteInfoLine("--", 1);
            ConsolePrinter.WriteInfoLine("Nodes executions: " + executionMetrics.TotalNodesExecuted, 1);
            ConsolePrinter.WriteInfoLine("Execution time: " + executionMetrics.TotalTime.ToString(), 1);
            Console.WriteLine();
            ConsolePrinter.WriteSuccessLine("Total leaf nodes passed: " + executionMetrics.TotalLeafNodesPassed);

            if (executionMetrics.TotalNodeErrors == 0)
                Console.WriteLine("No failures");
            else
                ConsolePrinter.WriteErrorLine("Total nodes with errors: " + executionMetrics.TotalNodeErrors);

            Console.WriteLine();

            if (executionMetrics.TotalNodeErrors == 0)
                ConsolePrinter.WriteSuccessLine("[ALL GREEN]");
            else
                ConsolePrinter.WriteErrorLine("[HAD SOME FAILURES]");

            Console.WriteLine();
        }
    }
}