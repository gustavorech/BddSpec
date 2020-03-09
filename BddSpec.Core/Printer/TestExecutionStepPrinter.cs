
using System;
using System.Linq;

namespace BddSpec.Core.Printer
{
    internal class TestExecutionStepPrinter
    {
        private static object _printerLock = new object();

        internal static void PrintVerboseOrStatus(TestExecutionStep executionStep)
        {
            lock (_printerLock)
            {
                if (ExecutionConfiguration.Verbosity == PrinterVerbosity.VerboseSteps)
                    TestExecutionStepPrinter.PrintVerbose(executionStep);
                else
                    PrintOnlyStatus(executionStep);
            }
        }

        internal static void PrintOnlyStatus(TestExecutionStep executionStep)
        {
            if (executionStep.IsHadError)
                ConsolePrinter.WriteError("F");
            else if (executionStep.IsLeaf)
                ConsolePrinter.WriteSuccess(".");
        }

        internal static void PrintVerbose(TestExecutionStep executionStep)
        {
            TestStepDescription testStepDescription = executionStep.TestStepDescription;

            ConsolePrinter.WriteIdentation(executionStep.StepLevel);

            PrintDescription(executionStep, testStepDescription);

            if (ExecutionConfiguration.ShowTime)
                PrintStepTimeSpan(executionStep.TotalTimeSpent);

            PrintException(executionStep.ErrorException, executionStep.StepLevel);

            Console.WriteLine();
        }

        private static void PrintDescription(TestExecutionStep testExecutionStep,
            TestStepDescription testStepDescription)
        {
            string message = $"{testStepDescription.ContextTypeName}: {testStepDescription.TestDescription}";

            if (ExecutionConfiguration.ShowLine)
                message = $"{message} :{testStepDescription.SourceFileNumber}";

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

        private static void PrintException(Exception ex, int stepLevel)
        {
            if (!ExecutionConfiguration.PrintExceptions || ex == null)
                return;

            Console.WriteLine();
            ex.Message
                ?.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .ToList()
                .ForEach(messageLine =>
                {
                    Console.WriteLine();
                    ConsolePrinter.WriteIdentation(stepLevel + 1);
                    ConsolePrinter.WriteError(messageLine);
                });

            Console.WriteLine();
            ex.StackTrace
                ?.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .ToList()
                .ForEach(messageLine =>
                {
                    Console.WriteLine();
                    ConsolePrinter.WriteIdentation(stepLevel);
                    ConsolePrinter.WriteInfo(messageLine);
                });

            Console.WriteLine();
        }
    }
}
