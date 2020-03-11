using System;
using System.Collections.Generic;
using System.Linq;
using BddSpec.Configuration;
using BddSpec.Execution;

namespace BddSpec.Printer
{
    public class ExecutionPrinter
    {
        public static void NotifyDiscovererInitialized()
        {
            ConsolePrinter.WriteInfoLine("Initializing.");
            ConsolePrinter.WriteInfoLine("Discovering spec classes.");
        }

        public static void NotifySpecDiscovererFilter(string filter)
        {
            Console.WriteLine();
            ConsolePrinter.WriteInfoLine("Filtering spec classes by: " + filter);
        }

        public static void NotifySpecsFiltered(List<Type> specClassesTypes)
        {
            Console.WriteLine();
            Console.WriteLine("Filtered spec classes:");

            specClassesTypes
                .Select(c => c.FullName)
                .ToList()
                .ForEach(c => ConsolePrinter.WriteSuccessLine("- " + c, 1));
        }

        public static void NotifyNoSpecClassesFound()
        {
            Console.WriteLine();
            ConsolePrinter.WriteErrorLine("No spec classes was found", 2);
            ConsolePrinter.WriteErrorLine("Aborting the execution", 2);
            Console.WriteLine();
        }

        public static void NotifyStartingExecution()
        {
            Console.WriteLine();
            ConsolePrinter.WriteInfoLine("Executing specs:");
        }

        public static void NotifyInitialized(SpecExecutor specExecutor)
        {
            bool separateSpecClassPrinterByOneLine = ExecutionConfiguration.IsPrintVerbose;

            if (ExecutionConfiguration.IsPrintVerbose)
                Console.WriteLine();
        }

        public static void NotifyInitialized(ExecutionStep executionStep)
        {
            if (!ExecutionConfiguration.IsPrintVerbose)
                return;

            if (executionStep.IsLeaf)
                return;

            TestExecutionStepPrinter.PrintVerbose(executionStep);
        }

        public static void NotifyError(ExecutionStep executionStep)
        {
            TestExecutionStepPrinter.PrintVerboseOrStatus(executionStep);
        }

        public static void NotifyCompleted(ExecutionStep executionStep)
        {
            if (executionStep.IsBranch)
                return;

            if (executionStep.IsHadError)
                return;

            TestExecutionStepPrinter.PrintVerboseOrStatus(executionStep);
        }
    }
}