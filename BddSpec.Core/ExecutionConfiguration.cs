
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    public class ExecutionConfiguration
    {
        public static PrinterVerbosity Verbosity = PrinterVerbosity.VerboseAfterCompletion;
        public static bool ExecuteAsynchronous { get; set; }
        public static bool PrintExceptions { get; set; }
        public static bool ShowLine { get; set; }
        public static bool ShowTime { get; set; }
    }
}
