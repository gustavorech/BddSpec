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

        internal void ClearSpecActionsToExecuteNextStep()
        {
            SpecActions.Clear();
        }

        public void AddSpecAction(string verb, string description, Action action,
            string sourceFilePath, int sourceLineNumber)
        {
            SpecDescription specDescription = new SpecDescription(
                sourceFilePath, sourceLineNumber, description, "when");

            SpecActions.Add(new SpecAction(specDescription, action));
        }

        public void When(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => AddSpecAction("when", description, action, sourceFilePath, sourceLineNumber);

        public void It(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => AddSpecAction("it", description, action, sourceFilePath, sourceLineNumber);

        public void Method(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => AddSpecAction("method", description, action, sourceFilePath, sourceLineNumber);

        public void Context(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => AddSpecAction("context", description, action, sourceFilePath, sourceLineNumber);

        public void Describe(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => AddSpecAction("describe", description, action, sourceFilePath, sourceLineNumber);

        public void Then(string description, Action action,
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => AddSpecAction("then", description, action, sourceFilePath, sourceLineNumber);
    }
}
