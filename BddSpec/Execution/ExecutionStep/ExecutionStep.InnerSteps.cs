using System.Linq;

using System.Collections.Generic;
using BddSpec.Configuration;

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

        private void AddInnerStepsFromActions(List<SpecAction> stepActions)
        {
            if (ExecutionConfiguration.IsSpecificLine)
                AddFilteredInnerStepsFromActions(stepActions);
            else
                AddAllInnerStepsFromActions(stepActions);
        }

        private void AddFilteredInnerStepsFromActions(List<SpecAction> stepActions)
        {
            if (stepActions.Count <= 1)
            {
                AddAllInnerStepsFromActions(stepActions);
                return;
            }

            int specificLine = ExecutionConfiguration.SpecificLine.Value;

            if (specificLine < stepActions.First().Description.SourceFileNumber)
            {
                AddAllInnerStepsFromActions(stepActions);
                return;
            }

            if (specificLine > stepActions.Last().Description.SourceFileNumber)
            {
                AddAllInnerStepsFromActions(stepActions);
                return;
            }

            for (int i = 0; i < stepActions.Count; i++)
            {
                SpecAction currentStepAction = stepActions[i];
                int currentLine = currentStepAction.Description.SourceFileNumber;

                if (currentLine == specificLine)
                {
                    AddInnerStep(stepActions[i], i);
                    return;
                }

                bool firstStepThatPassSpecificLine = currentLine > specificLine;

                if (firstStepThatPassSpecificLine)
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