
using System.Collections.Generic;

namespace BddSpec.Execution
{
    public partial class ExecutionStep
    {
        private List<ExecutionStep> _innerSteps = new List<ExecutionStep>();

        public bool IsLeaf { get => _innerSteps.Count == 0; }
        public bool IsBranch { get => _innerSteps.Count > 0; }

        public ExecutionStep GetNotCompletedInnerStepToExecute()
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