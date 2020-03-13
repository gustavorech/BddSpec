
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
            if (IsInitialized || IsFailed)
                return;

            ExecuteOnceBeforeIfHaveAndCleanIt(specClassInstance);

            if (IsFailed)
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
            if (IsBranchStep)
                return;

            while (specClassInstance.AfterActions.Count > 0 && !IsFailed)
                SafeExecuteAction(specClassInstance.AfterActions.Pop());

            NotifyCompletion();
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
                NotifyFailure(ex);
            }
            finally
            {
                timer.Stop();
                TotalTimesExecuted++;
                TotalTimeSpent += timer.Elapsed;
            }
        }
    }
}