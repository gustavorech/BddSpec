
using System.Collections.Generic;

namespace BddSpec.Core
{
    public partial class TestExecutionStep
    {
        private List<TestExecutionStep> _innerSteps = new List<TestExecutionStep>();

        public bool IsALeafStep { get => _innerSteps.Count == 0; }

        public TestExecutionStep GetCurrentStepToExecute()
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