using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using BddSpec.Printer;

namespace BddSpec.Execution
{
    public class SuiteExecutor
    {
        private static List<Type> specClassesTypes = new List<Type>();

        public static bool DiscoverAndExecute()
        {
            Stopwatch timer = Stopwatch.StartNew();

            bool isSuccessDiscovering = DiscoverSpecClasses();
            if (!isSuccessDiscovering)
                return false;

            List<SpecExecutor> specExecutors = ExecuteSyncOrASync();

            bool hasErrors = ActionsAfterExecutionCompleted(specExecutors);

            Console.WriteLine("Total time: " + timer.Elapsed.ToString());

            if (hasErrors)
                return false;

            return true;
        }

        private static bool DiscoverSpecClasses()
        {
            ExecutionPrinter.PrintDiscovererInitialized();

            specClassesTypes = SpecDiscoverer.FilteredSpecClassesTypes();

            if (!specClassesTypes.Any())
            {
                ExecutionPrinter.PrintNoSpecClassesFound();
                return false;
            }

            if (Configuration.IsSpecFiltered)
                ExecutionPrinter.PrintSpecsFiltered(specClassesTypes);

            if (Configuration.IsSpecificLine && specClassesTypes.Count > 1)
            {
                ExecutionPrinter.PrintErrorMoreThanOneSpecClassForSpecificLine();
                return false;
            }

            return true;
        }

        private static List<SpecExecutor> ExecuteSyncOrASync()
        {
            ExecutionPrinter.PrintExecutionSpecs();

            bool shouldBlockAsynchronous =
                Configuration.Verbosity == PrinterVerbosity.VerboseSteps;

            bool executeAsynchronous =
                Configuration.ExecuteAsynchronous && !shouldBlockAsynchronous;

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
            specExecutor.IsolateAndExecuteAllPaths();

            VerbosePrinter.NotifyCompleted(specExecutor);

            return specExecutor;
        }

        private static bool ActionsAfterExecutionCompleted(List<SpecExecutor> specExecutors)
        {
            ExecutionPrinter.PrintSuiteExecutionCompleted();

            VerifyPrintSummaryAtEnd(specExecutors);

            PrintErorrsIfOccurred(specExecutors);

            ExecutionMetrics executionMetrics = CollectAndPrintMetrics(specExecutors);

            return executionMetrics.TotalNodeWithFailures > 0;
        }

        private static void VerifyPrintSummaryAtEnd(List<SpecExecutor> testExecutors)
        {
            if (Configuration.Verbosity == PrinterVerbosity.VerboseAfterCompletion)
            {
                ExecutionPrinter.PrintShowingSummary();

                testExecutors
                    .ForEach(testExecutor =>
                    {
                        testExecutor.PrintSummary();
                        Console.WriteLine();
                    });
            }
        }

        private static void PrintErorrsIfOccurred(List<SpecExecutor> specExecutors)
        {
            Configuration.ShowLine = true;

            if (specExecutors.Any(c => c.IsBranchHadError))
            {
                ExecutionPrinter.PrintShowingFailureDetailed();
                specExecutors.ForEach(c => c.PrintErrorsDetailed());

                ExecutionPrinter.PrintShowingFailuresSummary();
                specExecutors
                    .Where(c => c.IsBranchHadError)
                    .ToList()
                    .ForEach(c =>
                    {
                        c.PrintErrorsSummary();
                        Console.WriteLine();
                    });

                ExecutionPrinter.PrintSpecClassesWithFailure(specExecutors);
            }
        }

        private static ExecutionMetrics CollectAndPrintMetrics(List<SpecExecutor> testExecutors)
        {
            ExecutionMetrics executionMetrics = new ExecutionMetrics();
            testExecutors.ForEach(c => c.CollectMetrics(executionMetrics));

            ExecutionMetricsPrinter.Print(executionMetrics);

            return executionMetrics;
        }
    }
}
