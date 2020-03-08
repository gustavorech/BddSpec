
using System;
using System.Diagnostics;
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    public partial class TestExecutionStep
    {
        private bool _isInitialized;

        public bool IsInitialized { get => _isInitialized; }

        public void Execute(TestStepAction stepAction, BddLike testClassInstance)
        {
            if (!IsInitialized)
                Initialize(stepAction, testClassInstance);
            else
                SafeExecute(stepAction);
        }

        private void Initialize(TestStepAction stepAction, BddLike testClassInstance)
        {
            int currentStackCount = testClassInstance.testStepsActions.Count;

            SafeExecute(stepAction);

            for (int i = currentStackCount; i < testClassInstance.testStepsActions.Count; i++)
            {
                TestStepAction innerTestAction = testClassInstance.testStepsActions[i];
                CreateInnerStepFromAction(innerTestAction, i);
            }

            NotifyInitialized();
        }

        private void NotifyInitialized()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            CentralizedPrinter.NotifyInitialized(this);

            if (_innerSteps.Count == 0)
                NotifyCompleted();
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
                TimesThisStepWasExecuted++;
                TotalTimeSpent += timer.Elapsed;
            }
        }
    }
}