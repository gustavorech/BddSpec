using System;
using BddSpec.Execution;

namespace BddSpec.Printer
{
    public class ExecutionMetricsPrinter
    {
        public static void Print(ExecutionMetrics executionMetrics)
        {
            Console.WriteLine();
            PrinterHelper.WriteInfoLine("Execution metrics:");
            PrinterHelper.WriteInfoLine("Test classes executed: " + executionMetrics.TotalSpecClasses, 1);
            PrinterHelper.WriteInfoLine("Nodes reached: " + executionMetrics.TotalNodesReached, 1);
            PrinterHelper.WriteInfoLine("Leaf nodes reached: " + executionMetrics.TotalLeafNodesReached, 1);
            PrinterHelper.WriteInfoLine("--", 1);
            PrinterHelper.WriteInfoLine("Nodes executions: " + executionMetrics.TotalTimesNodesWereExecuted, 1);
            PrinterHelper.WriteInfoLine("Execution time: " + executionMetrics.TotalExecutionTimeSpent.ToString(), 1);
            Console.WriteLine();
            PrinterHelper.WriteSuccessLine("Total leaf nodes passed: " + executionMetrics.TotalLeafNodesSucceeded);

            if (executionMetrics.TotalNodeWithFailures == 0)
                Console.WriteLine("No failures");
            else
                PrinterHelper.WriteErrorLine("Total nodes with errors: " + executionMetrics.TotalNodeWithFailures);

            Console.WriteLine();

            if (executionMetrics.TotalNodeWithFailures == 0)
                PrinterHelper.WriteSuccessLine("[ALL GREEN]");
            else
                PrinterHelper.WriteErrorLine("[HAD SOME FAILURES]");

            Console.WriteLine();
        }
    }
}