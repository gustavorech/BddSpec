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

            List<SpecClassExecutor> specExecutors = ExecuteSyncOrASync();

            bool hasErrors = ActionsAfterExecutionCompleted(specExecutors);

            ExecutionPrinter.PrintTotalTime(timer.Elapsed);

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

        private static List<SpecClassExecutor> ExecuteSyncOrASync()
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

        private static List<SpecClassExecutor> ExecuteSynchronous() =>
            specClassesTypes
                .Select(CreateAndExecuteTestClassExecutor)
                .ToList();

        private static List<SpecClassExecutor> ExecuteAsynchronous() =>
            specClassesTypes
                .AsParallel()
                .Select(CreateAndExecuteTestClassExecutor)
                .ToList();

        private static SpecClassExecutor CreateAndExecuteTestClassExecutor(Type type)
        {
            SpecClassExecutor specExecutor = new SpecClassExecutor(type);
            specExecutor.IsolateAndExecuteAllPaths();

            VerbosePrinter.NotifyCompleted(specExecutor);

            return specExecutor;
        }

        private static bool ActionsAfterExecutionCompleted(List<SpecClassExecutor> specExecutors)
        {
            ExecutionPrinter.PrintSuiteExecutionCompleted();

            VerifyPrintSummaryAtEnd(specExecutors);

            PrintErorrsIfOccurred(specExecutors);

            ExecutionMetrics executionMetrics = CollectAndPrintMetrics(specExecutors);

            return executionMetrics.TotalNodeWithFailures > 0;
        }

        private static void VerifyPrintSummaryAtEnd(List<SpecClassExecutor> testExecutors)
        {
            if (Configuration.Verbosity == PrinterVerbosity.VerboseAfterCompletion)
            {
                ExecutionPrinter.PrintShowingSummary();

                testExecutors
                    .ForEach(testExecutor =>
                    {
                        testExecutor.PrintSummary();
                        PrinterHelper.BlankLine();
                    });
            }
        }

        private static void PrintErorrsIfOccurred(List<SpecClassExecutor> specExecutors)
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
                        PrinterHelper.BlankLine();
                    });

                ExecutionPrinter.PrintSpecClassesWithFailure(specExecutors);
            }
        }

        private static ExecutionMetrics CollectAndPrintMetrics(List<SpecClassExecutor> testExecutors)
        {
            ExecutionMetrics executionMetrics = new ExecutionMetrics();
            testExecutors.ForEach(c => c.CollectMetrics(executionMetrics));

            ExecutionMetricsPrinter.Print(executionMetrics);

            return executionMetrics;
        }
    }
}
