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
                BddLike testClassInstance = (BddLike)Activator.CreateInstance(type);

                TestStepDescription stepDescription = new TestStepDescription("", 0, type.Name, TestStepType.Class);
                TestStepAction stepAction = new TestStepAction(stepDescription, testClassInstance.ConfigureTests);

                testClassInstance.testStepsActions.Add(stepAction);

                if (rootExecutionStep == null)
                    rootExecutionStep = new TestExecutionStep(null, stepAction, 0, 0);

                Recursion(testClassInstance, rootExecutionStep);
            }
            while (!rootExecutionStep.IsCompleted);
        }

        private void Recursion(BddLike testClassInstance, TestExecutionStep currentStep)
        {
            if (currentStep == null)
                return;

            TestStepAction currentAction =
                testClassInstance.testStepsActions[currentStep.PositionToGetTheActionInTheStack];

            currentStep.Execute(currentAction, testClassInstance);

            Recursion(testClassInstance, currentStep.GetCurrentStepToExecute());
        }

        public void Print()
        {
            Console.WriteLine();

            rootExecutionStep.Print();
        }

        public void PrintOnlyErrors()
        {
            if (rootExecutionStep.IsBranchHadError)
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
