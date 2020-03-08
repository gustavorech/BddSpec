
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

        public static void NotifyCompletion(TestExecutionStep testExecutionStep)
        {
            lock (_lock)
            {
                if (Strategy == PrinterStrategy.VerboseSteps)
                    TestExecutionStepPrinter.Print(testExecutionStep);
                else if (testExecutionStep.IsHadAnExecutionError)
                    ConsolePrinter.WriteError("F");
                else if (testExecutionStep.IsALeafStep)
                    ConsolePrinter.WriteSuccess(".");
            }
        }
    }
}
