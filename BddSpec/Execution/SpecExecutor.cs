using System;

namespace BddSpec.Execution
{
    public class SpecExecutor
    {
        private Type _type;
        private ExecutionStep _rootStep;

        public bool IsBranchHadError
        {
            get => _rootStep?.IsBranchHadError ?? false;
        }

        public SpecExecutor(Type type)
        {
            this._type = type;
        }

        public void IsolateAndExecuteAllPaths()
        {
            do
            {
                SpecClass specClassInstance = (SpecClass)Activator.CreateInstance(_type);

                SpecDescription stepDescription = new SpecDescription("", 0, _type.Name, "class");
                SpecAction stepAction = new SpecAction(stepDescription, specClassInstance.SetUpSpecs);

                specClassInstance.SpecActions.Add(stepAction);

                VerifyCreateRootStepOnFirstIteration(stepAction);

                RecursiveExecuteOnePathThroughCompletion(specClassInstance, _rootStep);
            }
            while (!_rootStep.IsCompleted);
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
                    specClassInstance.SpecActions[currentStep.PositionToGetTheActionInTheStack];

                currentStep.Execute(currentAction, specClassInstance);

                currentStep = currentStep.GetNotCompletedInnerStepToExecute();
            }
        }

        public void PrintAllVerbose()
        {
            Console.WriteLine();

            _rootStep.Print();
        }

        public void PrintOnlyErrors()
        {
            if (!_rootStep.IsBranchHadError)
                return;

            _rootStep.PrintOnlyErrors();
        }

        public void CollectMetrics(ExecutionMetrics metrics)
        {
            metrics.TotalTestClasses++;

            _rootStep.CollectMetrics(metrics);
        }
    }
}
