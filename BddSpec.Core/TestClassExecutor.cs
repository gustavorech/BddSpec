﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    public class TestClassExecutor
    {
        private Type type;
        private List<TestExecutionStep> stepsDeclaredOnTop = new List<TestExecutionStep>();

        public TestClassExecutor(Type type)
        {
            this.type = type;
        }

        public void Execute()
        {
            Initialize();

            if (stepsDeclaredOnTop.Count == 0)
                return;

            ExecuteEveryTestClass();
        }

        private void ExecuteEveryTestClass()
        {
            while (true)
            {
                BddLike testClassInstance = (BddLike)Activator.CreateInstance(type);
                testClassInstance.ConfigureTests();

                TestExecutionStep currentExecutionStep =
                    stepsDeclaredOnTop.FirstOrDefault(c => !c.IsExecutionCompleted);

                if (currentExecutionStep == null)
                    return;

                Recursion(testClassInstance, currentExecutionStep);
            }
        }

        private void Recursion(BddLike testClassInstance, TestExecutionStep currentTestStep)
        {
            if (currentTestStep == null)
                return;

            TestStepAction currentTestAction =
                testClassInstance.testStepsActions[currentTestStep.PositionToGetTheActionInTheStack];

            int currentStackCount = testClassInstance.testStepsActions.Count;
            currentTestStep.SafeInvokeAction(currentTestAction);

            if (currentTestStep.HadAnExecutionError)
            {
                CentralizedPrinter.NotifyCompletion(currentTestStep);
                return;
            }

            if (!currentTestStep.IsInnerActionsHadBeenDiscovered)
            {
                for (int i = currentStackCount; i < testClassInstance.testStepsActions.Count; i++)
                {
                    TestStepAction innerTestAction = testClassInstance.testStepsActions[i];
                    currentTestStep.CreateInnerExecutionStepFromAction(innerTestAction, i);
                }

                currentTestStep.IsInnerActionsHadBeenDiscovered = true;

                CentralizedPrinter.NotifyCompletion(currentTestStep);
                if (currentTestStep.IsExecutionCompleted)
                    return;
            }

            Recursion(testClassInstance, currentTestStep.GetNextStepToExecute());
        }

        private void Initialize()
        {
            BddLike testClassInstance = (BddLike)Activator.CreateInstance(type);

            testClassInstance.ConfigureTests();

            for (int i = 0; i < testClassInstance.testStepsActions.Count; i++)
            {
                TestStepAction stepAction = testClassInstance.testStepsActions[i];
                stepsDeclaredOnTop.Add(new TestExecutionStep(null, stepAction, i, 1));
            }
        }

        public void Print()
        {
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine(stepsDeclaredOnTop.First().TestContextDescription.SourceFilePath);
            Console.WriteLine("class: " + type.Name);

            stepsDeclaredOnTop.ForEach(c => c.Print());
        }

        public void PrintOnlyErrors()
        {
            Console.WriteLine();
            Console.WriteLine("ERRORS!!!");
            Console.WriteLine(stepsDeclaredOnTop.First().TestContextDescription.SourceFilePath);
            Console.WriteLine("class: " + type.Name);
            stepsDeclaredOnTop.ForEach(c => c.PrintOnlyErrors());
        }
    }
}