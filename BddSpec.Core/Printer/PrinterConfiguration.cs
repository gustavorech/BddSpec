
using System;

namespace BddSpec.Core.Printer
{
    public class PrinterConfiguration
    {
        public static PrinterStrategy Strategy = PrinterStrategy.VerboseAfterCompletion;
        public static bool PrintExceptions { get; set; }
        public static bool ShowLine { get; set; }
        public static bool ShowTime { get; set; }
    }
}
