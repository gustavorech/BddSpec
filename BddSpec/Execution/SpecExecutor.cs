using System;

namespace BddSpec.Execution
{
    internal class SpecExecutor
    {
        private Type _type;
        private ExecutionStep _rootStep;

        internal SpecExecutor(Type type)
        {
            this._type = type;
        }

        internal void IsolateAndExecuteAllPaths()
        {
            do
            {
                SpecClass specClassInstance = (SpecClass)Activator.CreateInstance(_type);

                SpecDescription stepDescription = new SpecDescription("", 0, _type.Name, SpecType.Class);
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

        internal void PrintAllVerbose()
        {
            Console.WriteLine();

            _rootStep.Print();
        }

        internal void PrintOnlyErrors()
        {
            if (!_rootStep.IsBranchHadError)
                return;

            Console.WriteLine();
            Console.WriteLine("ERRORS!!!");
            _rootStep.PrintOnlyErrors();
        }

        internal void CollectMetrics(ExecutionMetrics metrics)
        {
            metrics.TotalTestClasses++;

            _rootStep.CollectMetrics(metrics);
        }
    }
}
