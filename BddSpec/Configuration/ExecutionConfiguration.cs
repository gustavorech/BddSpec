
using BddSpec.Printer;

namespace BddSpec.Configuration
{
    public class ExecutionConfiguration
    {
        public static string SpecSelector { get; set; }
        public static PrinterVerbosity Verbosity = PrinterVerbosity.VerboseAfterCompletion;
        public static bool ExecuteAsynchronous { get; set; }
        public static bool PrintExceptions { get; set; }
        public static bool ShowLine { get; set; }
        public static bool ShowTime { get; set; }

        public static bool IsPrintVerbose { get => Verbosity == PrinterVerbosity.VerboseSteps; }
        public static bool IsSpecFiltered { get => !string.IsNullOrEmpty(SpecSelector); }
    }
}
