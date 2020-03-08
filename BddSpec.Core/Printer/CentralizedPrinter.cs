
using System;

namespace BddSpec.Core.Printer
{
    public class CentralizedPrinter
    {
        public static PrinterStrategy Strategy = PrinterStrategy.VerboseSteps;
        public static bool PrintExceptions { get; set; }
        public static bool ShowLine { get; set; }
        public static bool ShowTime { get; set; }
        private static object _lock = new object();

        public static void NotifyCompleted(TestExecutionStep executionStep)
        {
            if (!executionStep.IsALeafStep && !executionStep.IsHadError)
                return;

            lock (_lock)
            {
                if (Strategy == PrinterStrategy.VerboseSteps)
                    TestExecutionStepPrinter.Print(executionStep);
                else if (executionStep.IsHadError)
                    ConsolePrinter.WriteError("F");
                else if (executionStep.IsALeafStep)
                    ConsolePrinter.WriteSuccess(".");
            }
        }

        public static void NotifyInitialized(TestExecutionStep executionStep)
        {
            if (executionStep.IsCompleted)
                return;

            if (!executionStep.IsALeafStep && Strategy == PrinterStrategy.VerboseSteps)
                TestExecutionStepPrinter.Print(executionStep);
        }
    }
}
