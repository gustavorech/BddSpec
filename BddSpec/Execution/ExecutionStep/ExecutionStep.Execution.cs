
using System;
using System.Diagnostics;

namespace BddSpec.Execution
{
    public partial class ExecutionStep
    {
        public void Execute(SpecAction stepAction, SpecClass specClassInstance)
        {
            specClassInstance.ClearSpecActionsToExecuteNextStep();

            SafeExecuteAction(stepAction.Action);

            InitializeOnFirstExecution(specClassInstance);
        }

        private void InitializeOnFirstExecution(SpecClass specClassInstance)
        {
            if (IsInitialized || IsHadError)
                return;

            ExecuteOnceBeforeIfHaveAndCleanIt(specClassInstance);

            if (IsHadError)
                return;

            AddInnerStepsFromActions(specClassInstance.SpecActions);

            NotifyInitialized();

            IfIsALeafExecuteAftersAndComplete(specClassInstance);
        }

        private void ExecuteOnceBeforeIfHaveAndCleanIt(SpecClass specClassInstance)
        {
            if (specClassInstance.OnceBefore != null)
                SafeExecuteAction(specClassInstance.OnceBefore);

            specClassInstance.OnceBefore = null;
        }

        private void IfIsALeafExecuteAftersAndComplete(SpecClass specClassInstance)
        {
            if (IsBranch)
                return;

            while (specClassInstance.AfterActions.Count > 0 && !IsHadError)
                SafeExecuteAction(specClassInstance.AfterActions.Pop());

            NotifyCompleted();
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