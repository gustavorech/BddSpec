
using System.Collections.Generic;

namespace BddSpec.Execution
{
    internal partial class ExecutionStep
    {
        private List<ExecutionStep> _innerSteps = new List<ExecutionStep>();

        internal bool IsLeaf { get => _innerSteps.Count == 0; }
        internal bool IsBranch { get => _innerSteps.Count > 0; }

        internal ExecutionStep GetNotCompletedInnerStepToExecute()
        {
            if (_quantityOfInnerStepsCompleted >= _innerSteps.Count)
                return null;

            return _innerSteps[_quantityOfInnerStepsCompleted];
        }

        private void AddInnerStep(
            SpecAction stepAction, int positionThisActionWillBeFindOnTheStack)
        {
            ExecutionStep innerStepCreated = new ExecutionStep(this, stepAction,
                positionThisActionWillBeFindOnTheStack, StepLevel + 1);

            _innerSteps.Add(innerStepCreated);
        }

        private void AddInnerStepsFromActions(List<SpecAction> stepActions)
        {
            for (int i = 0; i < stepActions.Count; i++)
            {
                SpecAction innerTestAction = stepActions[i];
                AddInnerStep(innerTestAction, i);
            }
        }
    }
}