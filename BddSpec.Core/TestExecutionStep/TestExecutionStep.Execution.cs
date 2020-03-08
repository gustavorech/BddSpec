
using System;
using System.Diagnostics;
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    public partial class TestExecutionStep
    {
        private bool _isInitialized;

        public bool IsInitialized { get => _isInitialized; }

        public void Execute(TestStepAction stepAction, SpecClass specClassInstance)
        {
            specClassInstance.testStepsActions.Clear();
            SafeExecute(stepAction);

            if (!IsInitialized)
                CreateInnerStepsFromAddedActions(specClassInstance);
        }

        private void CreateInnerStepsFromAddedActions(SpecClass specClassInstance)
        {
            for (int i = 0; i < specClassInstance.testStepsActions.Count; i++)
            {
                TestStepAction innerTestAction = specClassInstance.testStepsActions[i];
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
            else if (!IsCompleted)
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