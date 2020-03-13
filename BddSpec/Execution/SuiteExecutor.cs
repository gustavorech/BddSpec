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
            ExecutionPrinter.NotifyDiscovererInitialized();

            specClassesTypes = SpecDiscoverer.FilteredSpecClassesTypes();

            if (!specClassesTypes.Any())
            {
                ExecutionPrinter.NotifyNoSpecClassesFound();
                return false;
            }

            if (ExecutionConfiguration.IsSpecFiltered)
                ExecutionPrinter.NotifySpecsFiltered(specClassesTypes);

            if (ExecutionConfiguration.IsSpecificLine && specClassesTypes.Count > 1)
            {
                ExecutionPrinter.NotifyOnlyOneStepForSpecificLine();
                return false;
            }

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
            specExecutor.IsolateAndExecuteAllPaths();

            ExecutionPrinter.NotifyCompleted(specExecutor);

            return specExecutor;
        }

        private static bool ActionsAfterExecutionCompleted(List<SpecExecutor> specExecutors)
        {
            ExecutionPrinter.NotifySuiteExecutionCompleted();

            VerifyPrintSummaryAtEnd(specExecutors);

            PrintErorrsIfOccurred(specExecutors);

            ExecutionMetrics executionMetrics = CollectAndPrintMetrics(specExecutors);

            return executionMetrics.TotalNodeWithFailures > 0;
        }

        private static void VerifyPrintSummaryAtEnd(List<SpecExecutor> testExecutors)
        {
            if (ExecutionConfiguration.Verbosity == PrinterVerbosity.VerboseAfterCompletion)
            {
                ExecutionPrinter.NotifyPrintingSummary();

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
            ExecutionConfiguration.ShowLine = true;

            if (specExecutors.Any(c => c.IsBranchHadError))
            {
                ExecutionPrinter.NotifyPrintingErrorDetailed();
                specExecutors.ForEach(c => c.PrintErrorsDetailed());

                ExecutionPrinter.NotifyPrintingErrorSummary();
                specExecutors
                    .Where(c => c.IsBranchHadError)
                    .ToList()
                    .ForEach(c =>
                    {
                        c.PrintErrorsSummary();
                        Console.WriteLine();
                    });

                ExecutionPrinter.PrintSpecClassesWithError(specExecutors);
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
