using System;
using BddSpec.Configuration;
using BddSpec.Execution;

namespace BddSpec.Printer
{
    public class ExecutionPrinter
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

            if (executionStep.IsLeaf)
                return;

            TestExecutionStepPrinter.PrintVerbose(executionStep);
        }

        public static void NotifyError(ExecutionStep executionStep)
        {
            TestExecutionStepPrinter.PrintVerboseOrStatus(executionStep);
        }

        public static void NotifyCompleted(ExecutionStep executionStep)
        {
            if (executionStep.IsBranch)
                return;

            TestExecutionStepPrinter.PrintVerboseOrStatus(executionStep);
        }
    }
}