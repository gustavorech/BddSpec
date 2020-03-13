using System;
using System.Collections.Generic;
using System.Linq;
using BddSpec.Execution;

namespace BddSpec.Printer
{
    public class ExecutionPrinter
    {
        public static void PrintDiscovererInitialized()
        {
            PrinterHelper.WriteInfoLine("> Initializing");
            PrinterHelper.WriteInfoLine("> Discovering spec classes");
        }

        public static void PrintSpecDiscovererFilter(string filter)
        {
            Console.WriteLine();
            PrinterHelper.WriteInfoLine("> Filtering spec classes by: " + filter);
        }

        public static void PrintSpecsFiltered(List<Type> specClassesTypes)
        {
            Console.WriteLine();
            Console.WriteLine("Filtered spec classes:");

            specClassesTypes
                .Select(c => c.FullName)
                .ToList()
                .ForEach(c => PrinterHelper.WriteSuccessLine("- " + c, 1));
        }

        public static void PrintNoSpecClassesFound()
        {
            Console.WriteLine();
            PrinterHelper.WriteErrorLine("No spec classes were found");
            PrinterHelper.WriteErrorLine("Aborting the execution");
            Console.WriteLine();
        }

        public static void PrintErrorMoreThanOneSpecClassForSpecificLine()
        {
            Console.WriteLine();
            PrinterHelper.WriteErrorLine("To use a specific line number you need to filter by only one spec class");
            PrinterHelper.WriteErrorLine("Aborting the execution");
            Console.WriteLine();
        }

        public static void PrintExecutionSpecs()
        {
            Console.WriteLine();
            PrinterHelper.WriteInfoLine("> Executing specs");
        }

        public static void PrintSuiteExecutionCompleted()
        {
            if (!Configuration.IsPrintVerbose)
            {
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public static void PrintShowingSummary()
        {
            PrinterHelper.WriteInfoLine("> Showing summary of the execution");
        }

        public static void PrintShowingFailureDetailed()
        {
            PrinterHelper.WriteInfoLine("> Showing detailed failures descriptions and StackTrace");
        }

        public static void PrintShowingFailuresSummary()
        {
            PrinterHelper.WriteInfoLine("> Showing summary of failures (see details above)");
        }

        public static void PrintSpecClassesWithFailure(List<SpecClassExecutor> specExecutors)
        {
            Console.WriteLine("Spec classes with failures:");

            specExecutors
                .Where(c => c.IsBranchHadError)
                .ToList()
                .ForEach(c => PrinterHelper.WriteErrorLine("- " + c.Type.FullName, 1));
        }

        public static void PrintUnknownFatalError(Exception exception)
        {
            Console.WriteLine();
            PrinterHelper.WriteErrorLine("Unknown fatal error");
            ExceptionPrinter.Print(exception);
        }
    }
}