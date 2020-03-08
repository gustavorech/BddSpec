using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    public class TestDiscoverer
    {
        public static void DiscoverAndExecute()
        {
            Stopwatch timer = Stopwatch.StartNew();

            Console.WriteLine("Initializing tests...");
            IEnumerable<Type> testTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                typeof(SpecClass).IsAssignableFrom(x)
                && !x.IsInterface
                && !x.IsAbstract);

            Console.WriteLine("Executing...");
            Console.WriteLine();
            List<TestClassExecutor> testExecutors = Asincronous(testTypes);

            VerifyPrintAll(testExecutors);

            PrinterConfiguration.PrintExceptions = true;
            PrinterConfiguration.ShowLine = true;

            testExecutors.ForEach(c => c.PrintOnlyErrors());

            CollectAndPrintMetrics(testExecutors);

            Console.WriteLine("Total time: " + timer.Elapsed.ToString());
        }

        private static List<TestClassExecutor> Sincronous(IEnumerable<Type> testTypes) =>
            testTypes
                .Select(type =>
                {
                    TestClassExecutor testExecutor = new TestClassExecutor(type);
                    testExecutor.Execute();

                    return testExecutor;
                })
                .ToList();

        private static List<TestClassExecutor> Asincronous(IEnumerable<Type> testTypes) =>
            testTypes
                .AsParallel()
                .Select(type =>
                {
                    TestClassExecutor testExecutor = new TestClassExecutor(type);
                    testExecutor.Execute();

                    return testExecutor;
                })
                .ToList();

        private static void VerifyPrintAll(List<TestClassExecutor> testExecutors)
        {
            if (PrinterConfiguration.Strategy == PrinterStrategy.VerboseAfterCompletion)
                testExecutors
                    .ForEach(testExecutor =>
                    {
                        testExecutor.Print();
                    });
        }

        private static void CollectAndPrintMetrics(List<TestClassExecutor> testExecutors)
        {
            Metrics metrics = new Metrics();
            testExecutors.ForEach(c => c.CollectMetrics(metrics));

            Console.WriteLine();
            Console.WriteLine("METRICS");
            Console.WriteLine("Test classes executed: " + metrics.TotalTestClasses);
            Console.WriteLine("Nodes reached: " + metrics.TotalNodesReached);
            Console.WriteLine("Leaf nodes reached: " + metrics.TotalLeafNodes);
            Console.WriteLine("--");
            Console.WriteLine("Nodes executions: " + metrics.TotalNodesExecuted);
            Console.WriteLine("Execution time: " + metrics.TotalTime.ToString());
            Console.WriteLine("--");
            ConsolePrinter.WriteSuccess("Total leaf nodes passed: " + metrics.TotalLeafNodesPassed);
            Console.WriteLine();
            ConsolePrinter.WriteError("Total nodes with errors: " + metrics.TotalNodeErrors);
            Console.WriteLine();
            Console.WriteLine();

            if (metrics.TotalNodeErrors == 0)
            {
                ConsolePrinter.WriteIdentation(20);
                ConsolePrinter.WriteSuccess("-- THAT'S IT, ALL GREEN! --");
                Console.WriteLine();
                Console.WriteLine();
            }
            else
            {
                ConsolePrinter.WriteIdentation(20);
                ConsolePrinter.WriteError($"you have {metrics.TotalNodeErrors} more things to do");
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
