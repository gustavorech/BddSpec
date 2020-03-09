
using System;

namespace BddSpec.Core.Printer
{
    public class TestExecutionStepPrinter
    {
        private static object _printerLock = new object();

        public static void PrintVerboseOrStatus(TestExecutionStep executionStep)
        {
            lock (_printerLock)
            {
                if (ExecutionConfiguration.Verbosity == PrinterVerbosity.VerboseSteps)
                    TestExecutionStepPrinter.PrintVerbose(executionStep);
                else
                    PrintOnlyStatus(executionStep);
            }
        }

        public static void PrintOnlyStatus(TestExecutionStep executionStep)
        {
            if (executionStep.IsHadError)
                ConsolePrinter.WriteError("F");
            else if (executionStep.IsALeafStep)
                ConsolePrinter.WriteSuccess(".");
        }

        public static void PrintVerbose(TestExecutionStep executionStep)
        {
            TestStepDescription testStepDescription = executionStep.TestStepDescription;

            ConsolePrinter.WriteIdentation(executionStep.StepLevel);

            PrintDescription(executionStep, testStepDescription);

            if (ExecutionConfiguration.ShowLine)
                ConsolePrinter.WriteInfo($" (ln:{testStepDescription.SourceFileNumber})");

            if (ExecutionConfiguration.ShowTime)
                PrintStepTimeSpan(executionStep.TotalTimeSpent);

            PrintException(executionStep.ErrorException);

            Console.WriteLine();
        }

        private static void PrintDescription(TestExecutionStep testExecutionStep,
            TestStepDescription testStepDescription)
        {
            string message = $"{testStepDescription.ContextTypeName}: {testStepDescription.TestDescription}";

            if (testExecutionStep.IsHadError)
                ConsolePrinter.WriteError(message);
            else if (testExecutionStep.IsALeafStep)
                ConsolePrinter.WriteSuccess(message);
            else
                Console.Write(message);
        }

        private static void PrintStepTimeSpan(TimeSpan time)
        {
            if (time > TimeSpan.FromMinutes(1))
                ConsolePrinter.WriteInfo($" ({time.ToString("mm")}:{time.ToString("ss")}minutes)");
            else if (time > TimeSpan.FromSeconds(1))
                ConsolePrinter.WriteInfo($" ({time.ToString("ss")}:{time.ToString("fff")}s)");
            else if (time > TimeSpan.FromMilliseconds(100))
                ConsolePrinter.WriteInfo(" (" + time.ToString("fff") + "ms)");
            else if (time > TimeSpan.FromMilliseconds(10))
                ConsolePrinter.WriteInfo(" (" + time.ToString("fff").Substring(1) + "ms)");
        }

        private static void PrintException(Exception ex)
        {
            if (!ExecutionConfiguration.PrintExceptions || ex == null)
                return;

            Console.WriteLine();
            ConsolePrinter.WriteError("Erro: " + ex.Message);
            Console.WriteLine();
            ConsolePrinter.WriteInfo("StackTrace: " + ex.StackTrace);
            Console.WriteLine();
        }
    }
}
