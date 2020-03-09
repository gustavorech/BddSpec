using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BddSpec.Core
{
    public abstract partial class SpecClass
    {
        internal List<TestStepAction> TestStepActions { get; } = new List<TestStepAction>();
        internal Stack<Action> AfterActions { get; } = new Stack<Action>();

        public abstract void SetUpSpecs();

        public Action After
        {
            set => AfterActions.Push(value);
        }

        public void When(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            TestStepDescription testDescription = new TestStepDescription(
                sourceFilePath, sourceLineNumber, description, TestStepType.When);

            TestStepActions.Add(new TestStepAction(testDescription, action));
        }

        public void It(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            TestStepDescription testDescription = new TestStepDescription(
                sourceFilePath, sourceLineNumber, description, TestStepType.It);

            TestStepActions.Add(new TestStepAction(testDescription, action));
        }

        public void Method(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            TestStepDescription testDescription = new TestStepDescription(
                sourceFilePath, sourceLineNumber, description, TestStepType.Method);

            TestStepActions.Add(new TestStepAction(testDescription, action));
        }
    }
}
