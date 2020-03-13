using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BddSpec
{
    public abstract partial class SpecClass
    {
        public List<SpecAction> SpecActions { get; } = new List<SpecAction>();
        public Stack<Action> AfterActions { get; } = new Stack<Action>();
        public Action OnceBefore { get; set; }

        public Action After { set => AfterActions.Push(value); }

        public abstract void SetupSpecs();

        public void ClearSpecActionsToExecuteNextStep()
        {
            SpecActions.Clear();
        }

        public void AddSpecAction(string verb, string description,
         Action action, int sourceLineNumber)
        {
            SpecDescription specDescription = new SpecDescription(
                sourceLineNumber, description, verb);

            SpecActions.Add(new SpecAction(specDescription, action));
        }

        public void When(string description, Action action,
            [CallerLineNumber] int sourceLineNumber = 0)
            => AddSpecAction("when", description, action, sourceLineNumber);

        public void It(string description, Action action,
            [CallerLineNumber] int sourceLineNumber = 0)
            => AddSpecAction("it", description, action, sourceLineNumber);

        public void Method(string description, Action action,
            [CallerLineNumber] int sourceLineNumber = 0)
            => AddSpecAction("method", description, action, sourceLineNumber);

        public void Context(string description, Action action,
            [CallerLineNumber] int sourceLineNumber = 0)
            => AddSpecAction("context", description, action, sourceLineNumber);

        public void Describe(string description, Action action,
            [CallerLineNumber] int sourceLineNumber = 0)
            => AddSpecAction("describe", description, action, sourceLineNumber);

        public void Then(string description, Action action,
            [CallerLineNumber] int sourceLineNumber = 0)
            => AddSpecAction("then", description, action, sourceLineNumber);
    }
}
