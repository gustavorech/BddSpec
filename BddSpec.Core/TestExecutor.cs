﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace bddlike
{
    public partial class TestExecutionStep
    {
        public TestExecutionStep Parent { get; }
        public List<TestExecutionStep> Children { get; } = new List<TestExecutionStep>();
        public int PositionInStack { get; }
        public int StepLevel { get; }
        public TestContextDescription TestContextDescription { get; }

        public int ExecutionTimes { get; private set; }
        public TimeSpan TimeSpent { get; private set; } = TimeSpan.Zero;
        public bool IsChildrenDiscovered { get; set; }

        public bool IsExecutionCompleted
        {
            get
            {
                if (ThisStepHadAnExecutionError)
                    return true;

                return IsChildrenDiscovered && Children.TrueForAll(c => c.IsExecutionCompleted);
            }
        }

        public bool BranchHasExecutionError
        {
            get
            {
                return ThisStepHadAnExecutionError || Children.Any(c => c.BranchHasExecutionError);
            }
        }

        public TestExecutionStep(TestExecutionStep parent, TestContext context, int positionInStack, int level)
        {
            Parent = parent;
            TestContextDescription = context.Description;
            PositionInStack = positionInStack;
            StepLevel = level;
        }

        public void AddChild(TestContext context, int positionInStack)
        {
            Children.Add(new TestExecutionStep(this, context, positionInStack, StepLevel + 1));
        }

        public void SafeInvoke(TestContext context)
        {
            Stopwatch timer = Stopwatch.StartNew();

            try
            {
                context.Action.Invoke();
            }
            catch
            {
                NotifyHadAnExecutionError();
            }
            finally
            {
                ExecutionTimes++;
                TimeSpent += timer.Elapsed;
                timer.Stop();
            }
        }

        public void Print()
        {
            TestExecutionStepPrinter.Print(this);

            Children.ForEach(c => c.Print());
        }

        public void PrintOnlyErrors()
        {
            if (!this.BranchHasExecutionError)
                return;

            TestExecutionStepPrinter.Print(this);

            Children.ForEach(c => c.PrintOnlyErrors());
        }
    }

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

            if (root.Count == 0)
                return;

            Iterate();
        }

        private void Iterate()
        {
            while (!root.TrueForAll(c => c.IsExecutionCompleted))
            {
                BddLike instance = (BddLike)Activator.CreateInstance(type);
                instance.ConfigureTests();

                Recursion(instance, root);
            }
        }

        private void Recursion(BddLike instance, List<TestExecutionStep> listToExecute)
        {
            TestExecutionStep currentTestExecutor = listToExecute.First(c => !c.IsExecutionCompleted);
            TestContext currentTestContext = instance.testContexts[currentTestExecutor.PositionInStack];

            int currentStackCount = instance.testContexts.Count;
            currentTestExecutor.SafeInvoke(currentTestContext);

            if (currentTestExecutor.ThisStepHadAnExecutionError)
            {
                CentralizedPrinter.NotifyCompletion(currentTestExecutor);
                return;
            }

            if (!currentTestExecutor.IsChildrenDiscovered)
            {
                for (int i = currentStackCount; i < instance.testContexts.Count; i++)
                {
                    TestContext childTestContext = instance.testContexts[i];
                    currentTestExecutor.AddChild(childTestContext, i);
                }

                currentTestExecutor.IsChildrenDiscovered = true;

                CentralizedPrinter.NotifyCompletion(currentTestExecutor);
                if (currentTestExecutor.IsExecutionCompleted)
                    return;
            }

            Recursion(instance, currentTestExecutor.Children);
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
            root.ForEach(c => c.PrintOnlyErrors());
        }
    }
}
