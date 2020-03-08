
using System;

namespace BddSpec.Core.Printer
{
    public class TestExecutionStepPrinter
    {
        public static void Print(TestExecutionStep testExecutionStep)
        {
            TestStepDescription testStepDescription = testExecutionStep.TestStepDescription;

            ConsolePrinter.WriteIdentation(testExecutionStep.StepLevel);

            PrintDescription(testExecutionStep, testStepDescription);

            if (CentralizedPrinter.ShowLine)
                ConsolePrinter.WriteInfo($" (ln:{testStepDescription.SourceFileNumber})");

            if (CentralizedPrinter.ShowTime)
                PrintStepTimeSpan(testExecutionStep.TotalTimeSpent);

            PrintException(testExecutionStep.ErrorException);

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
            if (!CentralizedPrinter.PrintExceptions || ex == null)
                return;

            Console.WriteLine();
            ConsolePrinter.WriteError("Erro: " + ex.Message);
            Console.WriteLine();
            ConsolePrinter.WriteInfo("StackTrace: " + ex.StackTrace);
            Console.WriteLine();
        }
    }
}
