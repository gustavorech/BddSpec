﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using BddSpec.Printer;
using BddSpec.Configuration;

namespace BddSpec.Execution
{
    public class SuiteExecutor
    {
        private static List<Type> specClassesTypes = new List<Type>();

        public static bool DiscoverAndExecute()
        {
            Stopwatch timer = Stopwatch.StartNew();

            ExecutionPrinter.NotifyDiscovererInitialized();

            specClassesTypes = SpecDiscoverer.FilteredSpecClassesTypes();

            if (!specClassesTypes.Any())
            {
                ExecutionPrinter.NotifyNoSpecClassesFound();
                return false;
            }

            if (ExecutionConfiguration.IsSpecFiltered)
                ExecutionPrinter.NotifySpecsFiltered(specClassesTypes);

            ExecutionPrinter.NotifyStartingExecution();

            List<SpecExecutor> specExecutors = ExecuteSyncOrASync();

            VerifyPrintSummaryAtEnd(specExecutors);

            PrintErorrsIfOccurred(specExecutors);

            bool hasErrors = CollectAndPrintMetrics(specExecutors);

            Console.WriteLine("Total time: " + timer.Elapsed.ToString());

            if (hasErrors)
                return false;

            return true;
        }

        private static List<SpecExecutor> ExecuteSyncOrASync()
        {
            bool shouldBlockAsynchronous =
                ExecutionConfiguration.Verbosity == PrinterVerbosity.VerboseSteps;

            if (ExecutionConfiguration.ExecuteAsynchronous && !shouldBlockAsynchronous)
                return ExecuteAsynchronous();
            else
                return ExecuteSynchronous();
        }

        private static List<SpecExecutor> ExecuteSynchronous() =>
            specClassesTypes
                .Select(CreateAndExecuteTestClassExecutor)
                .ToList();

        private static List<SpecExecutor> ExecuteAsynchronous() =>
            specClassesTypes
                .AsParallel()
                .Select(CreateAndExecuteTestClassExecutor)
                .ToList();

        private static SpecExecutor CreateAndExecuteTestClassExecutor(Type type)
        {
            SpecExecutor specExecutor = new SpecExecutor(type);

            ExecutionPrinter.NotifyInitialized(specExecutor);

            specExecutor.IsolateAndExecuteAllPaths();

            return specExecutor;
        }

        private static void VerifyPrintSummaryAtEnd(List<SpecExecutor> testExecutors)
        {
            if (ExecutionConfiguration.Verbosity == PrinterVerbosity.VerboseAfterCompletion)
                testExecutors
                    .ForEach(testExecutor =>
                    {
                        testExecutor.PrintAllVerbose();
                        Console.WriteLine();
                    });
        }

        private static void PrintErorrsIfOccurred(List<SpecExecutor> specExecutors)
        {
            ExecutionConfiguration.PrintExceptions = true;
            ExecutionConfiguration.ShowLine = true;

            specExecutors.ForEach(c => c.PrintOnlyErrors());
        }

        private static bool CollectAndPrintMetrics(List<SpecExecutor> testExecutors)
        {
            ExecutionMetrics metrics = new ExecutionMetrics();
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

            return metrics.TotalNodeErrors > 0;
        }
    }
}
