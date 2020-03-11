using System;
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

            bool isSuccessDiscovering = Discover();
            if (!isSuccessDiscovering)
                return false;

            List<SpecExecutor> specExecutors = ExecuteSyncOrASync();

            bool hasErrors = ActionsAfterExecution(specExecutors);

            Console.WriteLine("Total time: " + timer.Elapsed.ToString());

            if (hasErrors)
                return false;

            return true;
        }

        private static bool Discover()
        {
            ExecutionPrinter.NotifyDiscovererInitialized();

            specClassesTypes = SpecDiscoverer.FilteredSpecClassesTypes();

            if (!specClassesTypes.Any())
            {
                ExecutionPrinter.NotifyNoSpecClassesFound();
                return false;
            }

            if (ExecutionConfiguration.IsSpecFiltered)
                ExecutionPrinter.NotifySpecsFiltered(specClassesTypes);

            return true;
        }

        private static List<SpecExecutor> ExecuteSyncOrASync()
        {
            ExecutionPrinter.NotifyStartingExecution();

            bool shouldBlockAsynchronous =
                ExecutionConfiguration.Verbosity == PrinterVerbosity.VerboseSteps;

            bool executeAsynchronous =
                ExecutionConfiguration.ExecuteAsynchronous && !shouldBlockAsynchronous;

            if (executeAsynchronous)
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

        private static bool ActionsAfterExecution(List<SpecExecutor> specExecutors)
        {
            VerifyPrintSummaryAtEnd(specExecutors);

            PrintErorrsIfOccurred(specExecutors);

            ExecutionMetrics executionMetrics = CollectAndPrintMetrics(specExecutors);

            return executionMetrics.TotalNodeErrors > 0;
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

        private static ExecutionMetrics CollectAndPrintMetrics(List<SpecExecutor> testExecutors)
        {
            ExecutionMetrics executionMetrics = new ExecutionMetrics();
            testExecutors.ForEach(c => c.CollectMetrics(executionMetrics));

            ExecutionMetricsPrinter.NotifyMetricsCollected(executionMetrics);

            return executionMetrics;
        }
    }
}
