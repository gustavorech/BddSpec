
using System;
using System.Diagnostics;
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    internal partial class TestExecutionStep
    {
        private bool _isInitialized;

        internal bool IsInitialized { get => _isInitialized; }

        internal void Execute(TestStepAction stepAction, SpecClass specClassInstance)
        {
            specClassInstance.TestStepActions.Clear();
            SafeExecute(stepAction);

            if (!IsInitialized)
                CreateInnerStepsFromAddedActions(specClassInstance);
        }

        private void CreateInnerStepsFromAddedActions(SpecClass specClassInstance)
        {
            for (int i = 0; i < specClassInstance.TestStepActions.Count; i++)
            {
                TestStepAction innerTestAction = specClassInstance.TestStepActions[i];
                CreateInnerStepFromAction(innerTestAction, i);
            }

            NotifyInitialized();
        }

        private void NotifyInitialized()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            if (IsALeafStep)
                NotifyCompleted();

            if (IsCompleted || !IsALeafStep)
                TestExecutionStepPrinter.PrintVerboseOrStatus(this);
        }

        private void SafeExecute(TestStepAction stepAction)
        {
            Stopwatch timer = Stopwatch.StartNew();

            try
            {
                stepAction.Action.Invoke();
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