using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using BddSpec.Printer;

namespace BddSpec.Execution
{
    public class SuiteExecutor
    {
        private List<Type> specClassesTypes = new List<Type>();

        public bool DiscoverAndExecute()
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

        private bool DiscoverSpecClasses()
        {
            ExecutionPrinter.PrintDiscovererInitialized();

            SpecDiscoverer specDiscoverer = new SpecDiscoverer();
            specClassesTypes = specDiscoverer.FilteredSpecClassesTypes();

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

        private List<SpecClassExecutor> ExecuteSyncOrASync()
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

        private List<SpecClassExecutor> ExecuteSynchronous() =>
            specClassesTypes
                .Select(CreateAndExecuteTestClassExecutor)
                .ToList();

        private List<SpecClassExecutor> ExecuteAsynchronous() =>
            specClassesTypes
                .AsParallel()
                .Select(CreateAndExecuteTestClassExecutor)
                .ToList();

        private SpecClassExecutor CreateAndExecuteTestClassExecutor(Type type)
        {
            SpecClassExecutor specExecutor = new SpecClassExecutor(type);
            specExecutor.IsolateAndExecuteAllPaths();

            VerbosePrinter.NotifyCompleted(specExecutor);

            return specExecutor;
        }

        private bool ActionsAfterExecutionCompleted(List<SpecClassExecutor> specExecutors)
        {
            ExecutionPrinter.PrintSuiteExecutionCompleted();

            VerifyPrintSummaryAtEnd(specExecutors);

            PrintErorrsIfOccurred(specExecutors);

            ExecutionMetrics executionMetrics = CollectAndPrintMetrics(specExecutors);

            return executionMetrics.TotalNodeWithFailures > 0;
        }

        private void VerifyPrintSummaryAtEnd(List<SpecClassExecutor> testExecutors)
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

        private void PrintErorrsIfOccurred(List<SpecClassExecutor> specExecutors)
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

        private ExecutionMetrics CollectAndPrintMetrics(List<SpecClassExecutor> testExecutors)
        {
            ExecutionMetrics executionMetrics = new ExecutionMetrics();
            testExecutors.ForEach(c => c.CollectMetrics(executionMetrics));

            ExecutionMetricsPrinter.Print(executionMetrics);

            return executionMetrics;
        }
    }
}
