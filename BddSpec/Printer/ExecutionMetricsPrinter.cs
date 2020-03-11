using System;
using BddSpec.Execution;

namespace BddSpec.Printer
{
    public class ExecutionMetricsPrinter
    {
        public static void Print(ExecutionMetrics executionMetrics)
        {
            Console.WriteLine();
            Console.WriteLine("METRICS");
            Console.WriteLine("Test classes executed: " + executionMetrics.TotalTestClasses);
            Console.WriteLine("Nodes reached: " + executionMetrics.TotalNodesReached);
            Console.WriteLine("Leaf nodes reached: " + executionMetrics.TotalLeafNodes);
            Console.WriteLine("--");
            Console.WriteLine("Nodes executions: " + executionMetrics.TotalNodesExecuted);
            Console.WriteLine("Execution time: " + executionMetrics.TotalTime.ToString());
            Console.WriteLine("--");
            ConsolePrinter.WriteSuccess("Total leaf nodes passed: " + executionMetrics.TotalLeafNodesPassed);
            Console.WriteLine();
            ConsolePrinter.WriteError("Total nodes with errors: " + executionMetrics.TotalNodeErrors);
            Console.WriteLine();
            Console.WriteLine();

            if (executionMetrics.TotalNodeErrors == 0)
            {
                ConsolePrinter.WriteIdentation(20);
                ConsolePrinter.WriteSuccess("-- THAT'S IT, ALL GREEN! --");
                Console.WriteLine();
                Console.WriteLine();
            }
            else
            {
                ConsolePrinter.WriteIdentation(20);
                ConsolePrinter.WriteError($"you have {executionMetrics.TotalNodeErrors} more things to do");
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}