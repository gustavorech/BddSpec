
using System;
using System.Linq;
using BddSpec.Configuration;
using BddSpec.Execution;

namespace BddSpec.Printer
{
    public class TestExecutionStepPrinter
    {
        private static object _printerLock = new object();

        public static void PrintVerboseOrStatus(ExecutionStep executionStep)
        {
            lock (_printerLock)
            {
                if (ExecutionConfiguration.Verbosity == PrinterVerbosity.VerboseSteps)
                    TestExecutionStepPrinter.PrintVerbose(executionStep);
                else
                    PrintStatus(executionStep);
            }
        }

        public static void PrintStatus(ExecutionStep executionStep)
        {
            if (executionStep.IsHadError)
                ConsolePrinter.WriteError("F");
            else if (executionStep.IsLeaf)
                ConsolePrinter.WriteSuccess(".");
        }

        public static void PrintVerbose(ExecutionStep executionStep)
        {
            SpecDescription testStepDescription = executionStep.TestStepDescription;

            ConsolePrinter.WriteIdentation(executionStep.StepLevel);

            PrintDescription(executionStep, testStepDescription);

            if (ExecutionConfiguration.ShowTime)
                PrintStepTimeSpan(executionStep.TotalTimeSpent);

            Console.WriteLine();
        }

        private static void PrintDescription(ExecutionStep testExecutionStep,
            SpecDescription testStepDescription)
        {
            string message = $"{testStepDescription.Verb} {testStepDescription.TestDescription}";

            if (ExecutionConfiguration.ShowLine)
                message = $"{message}:{testStepDescription.SourceFileNumber}";

            if (testExecutionStep.IsHadError)
                ConsolePrinter.WriteError(message);
            else if (testExecutionStep.IsLeaf)
                ConsolePrinter.WriteSuccess(message);
            else
                Console.Write(message);
        }

        private static void PrintStepTimeSpan(TimeSpan time)
        {
            ConsolePrinter.WriteInfo(" (" + time.TotalMilliseconds + "ms)");
        }
    }
}
