using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BddSpec
{
    public abstract partial class SpecClass
    {
        internal List<SpecAction> SpecActions { get; } = new List<SpecAction>();
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
            SpecDescription testDescription = new SpecDescription(
                sourceFilePath, sourceLineNumber, description, SpecType.When);

            SpecActions.Add(new SpecAction(testDescription, action));
        }

        public void It(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            SpecDescription testDescription = new SpecDescription(
                sourceFilePath, sourceLineNumber, description, SpecType.It);

            SpecActions.Add(new SpecAction(testDescription, action));
        }

        public void Method(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            SpecDescription testDescription = new SpecDescription(
                sourceFilePath, sourceLineNumber, description, SpecType.Method);

            SpecActions.Add(new SpecAction(testDescription, action));
        }
    }
}
