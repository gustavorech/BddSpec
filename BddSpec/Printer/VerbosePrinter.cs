using System;
using BddSpec.Execution;

namespace BddSpec.Printer
{
    public class VerbosePrinter
    {
        public static void NotifyCompleted(SpecExecutor specExecutor)
        {
            bool separateSpecClassPrinterByOneLine = Configuration.IsPrintVerbose;

            if (Configuration.IsPrintVerbose)
                Console.WriteLine();
        }

        public static void NotifyInitialized(ExecutionStep executionStep)
        {
            if (!Configuration.IsPrintVerbose)
                return;

            if (executionStep.IsLeafStep)
                return;

            ExecutionStepPrinter.PrintVerbose(executionStep);
        }

        public static void NotifyError(ExecutionStep executionStep)
        {
            ExecutionStepPrinter.PrintVerboseOrStatus(executionStep);
        }

        public static void NotifyCompleted(ExecutionStep executionStep)
        {
            if (executionStep.IsBranchStep)
                return;

            if (executionStep.IsFailed)
                return;

            ExecutionStepPrinter.PrintVerboseOrStatus(executionStep);
        }
    }
}