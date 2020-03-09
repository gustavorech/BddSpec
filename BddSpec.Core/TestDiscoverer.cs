using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    internal class TestDiscoverer
    {
        private static List<Type> specClassesTypes = new List<Type>();

        internal static void DiscoverAndExecute()
        {
            Stopwatch timer = Stopwatch.StartNew();

            Console.WriteLine("Initializing tests...");
            Console.WriteLine("Executing...");
            Console.WriteLine();

            DiscoverSpecClassesTypes();

            List<TestClassExecutor> specExecutors = ExecuteSyncOrASync();

            VerifyPrintSummaryAtEnd(specExecutors);

            PrintErorrsIfOccurred(specExecutors);

            CollectAndPrintMetrics(specExecutors);

            Console.WriteLine("Total time: " + timer.Elapsed.ToString());
        }

        private static void DiscoverSpecClassesTypes()
        {
            specClassesTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                {
                    return typeof(SpecClass).IsAssignableFrom(x)
                    && !x.IsInterface
                    && !x.IsAbstract;
                })
                .ToList();

            if (!specClassesTypes.Any())
            {
                Console.WriteLine();
                Console.WriteLine();
                ConsolePrinter.WriteError("No spec classes was found");
                Environment.Exit(1);
            }
        }

        private static List<TestClassExecutor> ExecuteSyncOrASync()
        {
            bool shouldBlockAsynchronous =
                ExecutionConfiguration.Verbosity == PrinterVerbosity.VerboseSteps;

            if (ExecutionConfiguration.ExecuteAsynchronous && !shouldBlockAsynchronous)
                return ExecuteAsynchronous();
            else
                return ExecuteSynchronous();
        }

        private static List<TestClassExecutor> ExecuteSynchronous() =>
            specClassesTypes
                .Select(CreateAndExecuteTestClassExecutor)
                .ToList();

        private static List<TestClassExecutor> ExecuteAsynchronous() =>
            specClassesTypes
                .AsParallel()
                .Select(CreateAndExecuteTestClassExecutor)
                .ToList();

        private static TestClassExecutor CreateAndExecuteTestClassExecutor(Type type)
        {
            TestClassExecutor testExecutor = new TestClassExecutor(type);
            testExecutor.IsolateAndExecuteAllPaths();

            return testExecutor;
        }

        private static void VerifyPrintSummaryAtEnd(List<TestClassExecutor> testExecutors)
        {
            if (ExecutionConfiguration.Verbosity == PrinterVerbosity.VerboseAfterCompletion)
                testExecutors
                    .ForEach(testExecutor =>
                    {
                        testExecutor.PrintAllVerbose();
                    });
        }

        private static void PrintErorrsIfOccurred(List<TestClassExecutor> specExecutors)
        {
            ExecutionConfiguration.PrintExceptions = true;
            ExecutionConfiguration.ShowLine = true;

            specExecutors.ForEach(c => c.PrintOnlyErrors());
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
