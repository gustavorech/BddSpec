using System.Linq;
using System.Collections.Generic;

namespace BddSpec.Execution
{
    public partial class ExecutionStep
    {
        private List<ExecutionStep> _innerSteps = new List<ExecutionStep>();

        public bool IsLeafStep { get => _innerSteps.Count == 0; }
        public bool IsBranchStep { get => _innerSteps.Count > 0; }

        public ExecutionStep GetCurrentInnerStepToExecute()
        {
            if (_quantityOfInnerStepsCompleted >= _innerSteps.Count)
                return null;

            return _innerSteps[_quantityOfInnerStepsCompleted];
        }

        private void AddInnerStepsFromActions(List<SpecAction> stepActions)
        {
            bool shouldAddAllActions = VerifyIfShouldAddAllActions(stepActions);

            if (shouldAddAllActions)
                AddAllInnerStepsFromActions(stepActions);
            else
                AddActionInSpecificLineOrFirstActionBefore(stepActions);
        }

        private bool VerifyIfShouldAddAllActions(List<SpecAction> stepActions)
        {
            if (!Configuration.IsSpecificLine)
                return true;

            if (stepActions.Count <= 1)
                return true;

            int specificLine = Configuration.SpecificLine.Value;

            bool specificLineIsBeforeFirstActionLine =
                specificLine < stepActions.First().Description.SourceFileNumber;

            bool specificLineIsAfterLastActionLine =
                specificLine > stepActions.Last().Description.SourceFileNumber;

            return specificLineIsBeforeFirstActionLine
                || specificLineIsAfterLastActionLine;
        }

        private void AddActionInSpecificLineOrFirstActionBefore(List<SpecAction> stepActions)
        {
            int specificLine = Configuration.SpecificLine.Value;

            for (int i = 0; i < stepActions.Count; i++)
            {
                SpecAction currentStepAction = stepActions[i];
                int currentLine = currentStepAction.Description.SourceFileNumber;

                if (currentLine == specificLine)
                {
                    AddInnerStep(stepActions[i], i);
                    return;
                }

                bool isTheFirstStepThatPassSpecificLine = currentLine > specificLine;

                if (isTheFirstStepThatPassSpecificLine)
                {
                    AddInnerStep(stepActions[i - 1], i - 1);
                    return;
                }
            }
        }

        private void AddAllInnerStepsFromActions(List<SpecAction> stepActions)
        {
            for (int i = 0; i < stepActions.Count; i++)
            {
                SpecAction innerTestAction = stepActions[i];
                AddInnerStep(innerTestAction, i);
            }
        }

        private void AddInnerStep(
            SpecAction stepAction, int positionThisActionWillBeFindOnTheStack)
        {
            ExecutionStep innerStepCreated = new ExecutionStep(this, stepAction,
                positionThisActionWillBeFindOnTheStack, StepLevel + 1);

            _innerSteps.Add(innerStepCreated);
        }
    }
}