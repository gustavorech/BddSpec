
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

            SafeExecute(stepAction.Action);

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

            IfIsALeafExecuteAfters(specClassInstance);

            NotifyInitialized();
        }

        private void IfIsALeafExecuteAfters(SpecClass specClassInstance)
        {
            if (!IsLeaf)
                return;

            while (specClassInstance.AfterActions.Count > 0 && !IsHadError)
                SafeExecute(specClassInstance.AfterActions.Pop());
        }

        private void SafeExecute(Action action)
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