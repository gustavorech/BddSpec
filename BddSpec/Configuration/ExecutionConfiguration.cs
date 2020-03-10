﻿
using BddSpec.Printer;

namespace BddSpec.Configuration
{
    internal class ExecutionConfiguration
    {
        internal static string SpecSelector { get; set; }
        internal static PrinterVerbosity Verbosity = PrinterVerbosity.VerboseAfterCompletion;
        internal static bool ExecuteAsynchronous { get; set; }
        internal static bool PrintExceptions { get; set; }
        internal static bool ShowLine { get; set; }
        internal static bool ShowTime { get; set; }
    }
}