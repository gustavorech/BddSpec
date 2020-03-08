
using System;

namespace BddSpec.Core.Printer
{
    public class CentralizedPrinter
    {
        public static PrinterStrategy Strategy = PrinterStrategy.VerboseAfterCompletion;
        public static bool PrintExceptions { get; set; }
        public static bool ShowLine { get; set; }
        public static bool ShowTime { get; set; }
        private static object _lock = new object();

        public static void NotifyCompleted(TestExecutionStep executionStep)
        {
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
            if (Strategy == PrinterStrategy.VerboseSteps && !executionStep.IsALeafStep)
                TestExecutionStepPrinter.Print(executionStep);
        }
    }
}
