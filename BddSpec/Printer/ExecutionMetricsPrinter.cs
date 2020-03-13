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
            ConsolePrinter.WriteInfoLine("Test classes executed: " + executionMetrics.TotalSpecClasses, 1);
            ConsolePrinter.WriteInfoLine("Nodes reached: " + executionMetrics.TotalNodesReached, 1);
            ConsolePrinter.WriteInfoLine("Leaf nodes reached: " + executionMetrics.TotalLeafNodesReached, 1);
            ConsolePrinter.WriteInfoLine("--", 1);
            ConsolePrinter.WriteInfoLine("Nodes executions: " + executionMetrics.TotalTimesNodesWereExecuted, 1);
            ConsolePrinter.WriteInfoLine("Execution time: " + executionMetrics.TotalExecutionTimeSpent.ToString(), 1);
            Console.WriteLine();
            ConsolePrinter.WriteSuccessLine("Total leaf nodes passed: " + executionMetrics.TotalLeafNodesSucceeded);

            if (executionMetrics.TotalNodeWithFailures == 0)
                Console.WriteLine("No failures");
            else
                ConsolePrinter.WriteErrorLine("Total nodes with errors: " + executionMetrics.TotalNodeWithFailures);

            Console.WriteLine();

            if (executionMetrics.TotalNodeWithFailures == 0)
                ConsolePrinter.WriteSuccessLine("[ALL GREEN]");
            else
                ConsolePrinter.WriteErrorLine("[HAD SOME FAILURES]");

            Console.WriteLine();
        }
    }
}