
using BddSpec.Printer;

namespace BddSpec
{
    public class Configuration
    {
        public static string SpecFilter { get; set; }
        public static int? SpecificLine { get; set; }
        public static PrinterVerbosity Verbosity = PrinterVerbosity.VerboseAfterCompletion;
        public static bool ExecuteAsynchronous { get; set; }
        public static bool ShowLine { get; set; }
        public static bool ShowTime { get; set; }

        public static bool IsPrintVerbose { get => Verbosity == PrinterVerbosity.VerboseSteps; }
        public static bool IsSpecFiltered { get => !string.IsNullOrEmpty(SpecFilter); }
        public static bool IsSpecificLine { get => SpecificLine.HasValue; }
    }
}
