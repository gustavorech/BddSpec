
using System;
using BddSpec.Execution;

namespace BddSpec.Printer
{
    public class ExecutionStepPrinter
    {
        private static object _printerLock = new object();

        public static void PrintVerboseOrStatus(ExecutionStep executionStep)
        {
            lock (_printerLock)
            {
                if (Configuration.Verbosity == PrinterVerbosity.VerboseSteps)
                    ExecutionStepPrinter.PrintVerbose(executionStep);
                else
                    PrintStatus(executionStep);
            }
        }

        public static void PrintStatus(ExecutionStep executionStep)
        {
            if (executionStep.IsFailed)
                PrinterHelper.WriteError("F");
            else if (executionStep.IsLeafStep)
                PrinterHelper.WriteSuccess(".");
        }

        public static void PrintVerbose(ExecutionStep executionStep)
        {
            SpecDescription testStepDescription = executionStep.TestStepDescription;

            PrinterHelper.WriteIdentation(executionStep.StepLevel);

            PrintDescription(executionStep, testStepDescription);

            if (Configuration.ShowTime)
                PrintStepTimeSpan(executionStep.TotalTimeSpent);

            Console.WriteLine();
        }

        private static void PrintDescription(ExecutionStep testExecutionStep,
            SpecDescription testStepDescription)
        {
            string message = $"{testStepDescription.Verb} {testStepDescription.TestDescription}";

            if (Configuration.ShowLine && testStepDescription.SourceFileNumber != 0)
                message = $"{message}:{testStepDescription.SourceFileNumber}";

            if (testExecutionStep.IsFailed)
                PrinterHelper.WriteError(message);
            else if (testExecutionStep.IsLeafStep)
                PrinterHelper.WriteSuccess(message);
            else
                Console.Write(message);
        }

        private static void PrintStepTimeSpan(TimeSpan time)
        {
            PrinterHelper.WriteInfo(" (" + time.TotalMilliseconds + "ms)");
        }
    }
}
