using System;

namespace BddSpec.Execution
{
    public class SpecClassExecutor
    {
        private Type _type;
        private ExecutionStep _rootStep;

        public Type Type { get => _type; }

        public bool IsBranchHadError
        {
            get => _rootStep?.IsBAnyInBranchFailed ?? false;
        }

        public SpecClassExecutor(Type type)
        {
            this._type = type;
        }

        public void IsolateAndExecuteAllPaths()
        {
            do
            {
                try
                {
                    SpecClass specClassInstance = (SpecClass)Activator.CreateInstance(_type);

                    SpecDescription stepDescription = new SpecDescription(0, _type.FullName, "");
                    SpecAction stepAction = new SpecAction(stepDescription, specClassInstance.SetupSpecs);

                    specClassInstance.SpecActions.Add(stepAction);

                    CreateRootStepOnFirstIteration(stepAction);

                    RecursiveExecuteOnePathThroughCompletion(specClassInstance, _rootStep);
                }
                catch (Exception ex)
                {
                    ExecutionError(ex);
                }
            }
            while (!_rootStep.IsCompleted);
        }

        private void ExecutionError(Exception ex)
        {
            if (_rootStep == null)
            {
                SpecDescription stepDescription = new SpecDescription(0, _type.FullName, null);
                SpecAction stepAction = new SpecAction(stepDescription, () => { });
                _rootStep = new ExecutionStep(null, stepAction, 0, 0);
            }

            _rootStep.NotifyFailure(ex);
        }

        private void CreateRootStepOnFirstIteration(SpecAction stepAction)
        {
            if (_rootStep == null)
                _rootStep = new ExecutionStep(null, stepAction, 0, 0);
        }

        private void RecursiveExecuteOnePathThroughCompletion(SpecClass specClassInstance, ExecutionStep currentStep)
        {
            while (currentStep != null)
            {
                SpecAction currentAction =
                    specClassInstance.SpecActions[currentStep.PositionOfTheActionOnSpecClass];

                currentStep.Execute(currentAction, specClassInstance);

                currentStep = currentStep.GetCurrentInnerStepToExecute();
            }
        }

        public void PrintSummary()
        {
            _rootStep.PrintSummary();
        }

        public void PrintErrorsDetailed()
        {
            if (!_rootStep.IsBAnyInBranchFailed)
                return;

            _rootStep.PrintErrorsDetailed();
        }

        public void PrintErrorsSummary()
        {
            if (!_rootStep.IsBAnyInBranchFailed)
                return;

            _rootStep.PrintErrorsSummary();
        }

        public void CollectMetrics(ExecutionMetrics metrics)
        {
            metrics.TotalSpecClasses++;

            _rootStep.CollectMetrics(metrics);
        }
    }
}
