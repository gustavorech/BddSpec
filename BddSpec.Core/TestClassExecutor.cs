using System;

namespace BddSpec.Core
{
    public class TestClassExecutor
    {
        private Type type;
        private TestExecutionStep rootExecutionStep;

        public TestClassExecutor(Type type)
        {
            this.type = type;
        }

        public void Execute()
        {
            do
            {
                SpecClass specClassInstance = (SpecClass)Activator.CreateInstance(type);

                TestStepDescription stepDescription = new TestStepDescription("", 0, type.Name, TestStepType.Class);
                TestStepAction stepAction = new TestStepAction(stepDescription, specClassInstance.SetUpSpecs);

                specClassInstance.testStepsActions.Add(stepAction);

                if (rootExecutionStep == null)
                    rootExecutionStep = new TestExecutionStep(null, stepAction, 0, 0);

                ExecuteOnePath(specClassInstance, rootExecutionStep);
            }
            while (!rootExecutionStep.IsCompleted);
        }

        private void ExecuteOnePath(SpecClass specClassInstance, TestExecutionStep currentStep)
        {
            while (currentStep != null)
            {
                TestStepAction currentAction =
                    specClassInstance.testStepsActions[currentStep.PositionToGetTheActionInTheStack];

                currentStep.Execute(currentAction, specClassInstance);

                currentStep = currentStep.GetCurrentInnerStepToExecute();
            }
        }

        public void Print()
        {
            Console.WriteLine();

            rootExecutionStep.Print();
        }

        public void PrintOnlyErrors()
        {
            if (!rootExecutionStep.IsBranchHadError)
                return;

            Console.WriteLine();
            Console.WriteLine("ERRORS!!!");
            rootExecutionStep.PrintOnlyErrors();
        }

        public void CollectMetrics(Metrics metrics)
        {
            metrics.TotalTestClasses++;

            rootExecutionStep.CollectMetrics(metrics);
        }
    }
}
