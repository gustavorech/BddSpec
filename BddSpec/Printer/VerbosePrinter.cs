using System;
using BddSpec.Configuration;
using BddSpec.Execution;

namespace BddSpec.Printer
{
    public class VerbosePrinter
    {
        public static void NotifyCompleted(SpecExecutor specExecutor)
        {
            bool separateSpecClassPrinterByOneLine = ExecutionConfiguration.IsPrintVerbose;

            if (ExecutionConfiguration.IsPrintVerbose)
                Console.WriteLine();
        }

        public static void NotifyInitialized(ExecutionStep executionStep)
        {
            if (!ExecutionConfiguration.IsPrintVerbose)
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