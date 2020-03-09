
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    internal class ExecutionConfiguration
    {
        internal static PrinterVerbosity Verbosity = PrinterVerbosity.VerboseAfterCompletion;
        internal static bool ExecuteAsynchronous { get; set; }
        internal static bool PrintExceptions { get; set; }
        internal static bool ShowLine { get; set; }
        internal static bool ShowTime { get; set; }
    }
}
