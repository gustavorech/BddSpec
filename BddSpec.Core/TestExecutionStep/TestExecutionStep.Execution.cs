
using System;
using System.Diagnostics;
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    internal partial class TestExecutionStep
    {
        internal void Execute(TestStepAction stepAction, SpecClass specClassInstance)
        {
            specClassInstance.TestStepActions.Clear();

            SafeExecuteAction(stepAction.Action);

            InitializeOnFirstExecution(specClassInstance);
        }

        private void InitializeOnFirstExecution(SpecClass specClassInstance)
        {
            if (IsInitialized)
                return;

            CreateInnerStepsFromActions(specClassInstance);

            NotifyInitialized();

            ExecutePostInitializationActions(specClassInstance);
        }

        private void ExecutePostInitializationActions(SpecClass specClassInstance)
        {
            IfIsALeafExecuteAftersAndComplete(specClassInstance);

            VerifyPrintAfterInitialization();
        }

        private void IfIsALeafExecuteAftersAndComplete(SpecClass specClassInstance)
        {
            if (IsBranch)
                return;

            while (specClassInstance.AfterActions.Count > 0 && !IsHadError)
                SafeExecuteAction(specClassInstance.AfterActions.Pop());

            NotifyCompleted();
        }

        private void VerifyPrintAfterInitialization()
        {
            bool printIfIsABranchWithNoErorrsInitialization = IsBranch && !IsHadError;
            bool printIfIsCompletedOnInitialization = IsCompleted;

            if (printIfIsABranchWithNoErorrsInitialization || printIfIsCompletedOnInitialization)
                TestExecutionStepPrinter.PrintVerboseOrStatus(this);
        }

        private void SafeExecuteAction(Action action)
        {
            Stopwatch timer = Stopwatch.StartNew();

            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                NotifyHadError(ex);
            }
            finally
            {
                timer.Stop();
                TimesExecuted++;
                TotalTimeSpent += timer.Elapsed;
            }
        }
    }
}