using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace bddlike
{
    public class TestExecutor
    {
        private Type type;
        private int executed = 0;
        private List<TestExecutionStep> root = new List<TestExecutionStep>();

        public TestExecutor(Type type)
        {
            this.type = type;
        }

        public void Execute()
        {
            Initialize();
            Iterate();
        }

        private void Iterate()
        {
            if (root.Count == 0)
                return;

            while (true)
            {
                BddLike instance = (BddLike)Activator.CreateInstance(type);
                instance.ConfigureTests();

                TestExecutionStep currentTestStep =
                    root.FirstOrDefault(c => !c.IsExecutionCompleted);

                if (currentTestStep == null)
                    return;

                Recursion(instance, currentTestStep);
            }
        }

        private void Recursion(BddLike instance, TestExecutionStep currentTestStep)
        {
            if (currentTestStep == null)
                return;

            TestContext currentTestContext = instance.testContexts[currentTestStep.PositionInStack];

            int currentStackCount = instance.testContexts.Count;
            currentTestStep.SafeInvoke(currentTestContext);

            if (currentTestStep.ThisStepHadAnExecutionError)
            {
                CentralizedPrinter.NotifyCompletion(currentTestStep);
                return;
            }

            if (!currentTestStep.IsChildrenDiscovered)
            {
                for (int i = currentStackCount; i < instance.testContexts.Count; i++)
                {
                    TestContext childTestContext = instance.testContexts[i];
                    currentTestStep.AddChild(childTestContext, i);
                }

                currentTestStep.IsChildrenDiscovered = true;

                CentralizedPrinter.NotifyCompletion(currentTestStep);
                if (currentTestStep.IsExecutionCompleted)
                    return;
            }

            Recursion(instance, currentTestStep.GetNextStepToExecute());
        }

        private void Initialize()
        {
            BddLike instance = (BddLike)Activator.CreateInstance(type);

            instance.ConfigureTests();

            for (int i = executed; i < instance.testContexts.Count; i++)
            {
                TestContext testContext = instance.testContexts[i];
                root.Add(new TestExecutionStep(null, testContext, i, 1));
            }
        }

        public void Print()
        {
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine(root.First().TestContextDescription.SourceFilePath);
            Console.WriteLine("class: " + type.Name);

            root.ForEach(c => c.Print());
        }

        public void PrintOnlyErrors()
        {
            Console.WriteLine();
            Console.WriteLine("ERRORS!!!");
            Console.WriteLine(root.First().TestContextDescription.SourceFilePath);
            Console.WriteLine("class: " + type.Name);
            root.ForEach(c => c.PrintOnlyErrors());
        }
    }
}
