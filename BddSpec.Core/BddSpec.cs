using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BddSpec.Core
{
    public abstract partial class SpecClass
    {
        public List<TestStepAction> testStepsActions { get; } = new List<TestStepAction>();

        public abstract void SetUpSpecs();

        public void When(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            TestStepDescription testDescription = new TestStepDescription(
                sourceFilePath, sourceLineNumber, description, TestStepType.When);

            testStepsActions.Add(new TestStepAction(testDescription, action));
        }

        public void It(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            TestStepDescription testDescription = new TestStepDescription(
                sourceFilePath, sourceLineNumber, description, TestStepType.It);

            testStepsActions.Add(new TestStepAction(testDescription, action));
        }

        public void Method(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            TestStepDescription testDescription = new TestStepDescription(
                sourceFilePath, sourceLineNumber, description, TestStepType.Method);

            testStepsActions.Add(new TestStepAction(testDescription, action));
        }
    }
}
