using System;

namespace BddSpec.Execution
{
    public class SpecExecutor
    {
        private Type _type;
        private ExecutionStep _rootStep;

        public Type Type { get => _type; }

        public bool IsBranchHadError
        {
            get => _rootStep?.IsBAnyInBranchFailed ?? false;
        }

        public SpecExecutor(Type type)
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

                    SpecDescription stepDescription = new SpecDescription("", 0, _type.Name, "class");
                    SpecAction stepAction = new SpecAction(stepDescription, specClassInstance.SetUpSpecs);

                    specClassInstance.SpecActions.Add(stepAction);

                    VerifyCreateRootStepOnFirstIteration(stepAction);

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
                SpecDescription stepDescription = new SpecDescription("", 0, _type.Name, "class");
                SpecAction stepAction = new SpecAction(stepDescription, () => { });
                _rootStep = new ExecutionStep(null, stepAction, 0, 0);
            }

            _rootStep.NotifyFailure(ex);
        }

        private void VerifyCreateRootStepOnFirstIteration(SpecAction stepAction)
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
