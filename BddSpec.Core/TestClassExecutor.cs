using System;

namespace BddSpec.Core
{
    public class TestClassExecutor
    {
        private Type _type;
        private TestExecutionStep _rootStep;

        public TestClassExecutor(Type type)
        {
            this._type = type;
        }

        public void IsolateAndExecuteAllPaths()
        {
            do
            {
                SpecClass specClassInstance = (SpecClass)Activator.CreateInstance(_type);

                TestStepDescription stepDescription = new TestStepDescription("", 0, _type.Name, TestStepType.Class);
                TestStepAction stepAction = new TestStepAction(stepDescription, specClassInstance.SetUpSpecs);

                specClassInstance.testStepsActions.Add(stepAction);

                VerifyInitalizeRootStructureOnFirstPath(stepAction);

                ExecuteOnePathThroughCompletion(specClassInstance, _rootStep);
            }
            while (!_rootStep.IsCompleted);
        }

        private void VerifyInitalizeRootStructureOnFirstPath(TestStepAction stepAction)
        {
            if (_rootStep == null)
                _rootStep = new TestExecutionStep(null, stepAction, 0, 0);
        }

        private void ExecuteOnePathThroughCompletion(SpecClass specClassInstance, TestExecutionStep currentStep)
        {
            while (currentStep != null)
            {
                TestStepAction currentAction =
                    specClassInstance.testStepsActions[currentStep.PositionToGetTheActionInTheStack];

                currentStep.Execute(currentAction, specClassInstance);

                currentStep = currentStep.GetCurrentInnerStepToExecute();
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

            Console.WriteLine();
            Console.WriteLine("ERRORS!!!");
            _rootStep.PrintOnlyErrors();
        }

        public void CollectMetrics(Metrics metrics)
        {
            metrics.TotalTestClasses++;

            _rootStep.CollectMetrics(metrics);
        }
    }
}
