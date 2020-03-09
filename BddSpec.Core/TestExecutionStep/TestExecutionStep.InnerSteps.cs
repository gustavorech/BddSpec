
using System.Collections.Generic;

namespace BddSpec.Core
{
    internal partial class TestExecutionStep
    {
        private List<TestExecutionStep> _innerSteps = new List<TestExecutionStep>();

        internal bool IsLeaf { get => _innerSteps.Count == 0; }

        internal TestExecutionStep GetCurrentInnerStepToExecute()
        {
            if (_quantityOfInnerStepsCompleted >= _innerSteps.Count)
                return null;

            return _innerSteps[_quantityOfInnerStepsCompleted];
        }

        private void CreateInnerStepFromAction(
            TestStepAction stepAction, int positionThisActionWillBeFindOnTheStack)
        {
            TestExecutionStep innerStepCreated = new TestExecutionStep(this, stepAction,
                positionThisActionWillBeFindOnTheStack, StepLevel + 1);

            _innerSteps.Add(innerStepCreated);
        }
    }
}