
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace bddlike
{
    public partial class TestExecutionStep
    {
        private TestExecutionStep _parent;
        private List<TestExecutionStep> _children = new List<TestExecutionStep>();
        public int PositionInStack { get; }
        public int StepLevel { get; }
        public TestContextDescription TestContextDescription { get; }

        public int TimesThisStepWasExecuted { get; private set; }
        public TimeSpan TotalTimeSpent { get; private set; } = TimeSpan.Zero;
        public bool IsChildrenDiscovered { get; set; }
        public bool IsALeafStep { get => _children.Count == 0; }

        public bool IsExecutionCompleted
        {
            get
            {
                if (ThisStepHadAnExecutionError)
                    return true;

                return IsChildrenDiscovered && _children.TrueForAll(c => c.IsExecutionCompleted);
            }
        }

        public TestExecutionStep(TestExecutionStep parent, TestContext context, int positionInStack, int level)
        {
            this._parent = parent;
            TestContextDescription = context.Description;
            PositionInStack = positionInStack;
            StepLevel = level;
        }

        public void AddChild(TestContext context, int positionInStack)
        {
            _children.Add(new TestExecutionStep(this, context, positionInStack, StepLevel + 1));
        }

        public TestExecutionStep GetNextStepToExecute()
        {
            return _children.FirstOrDefault(c => !c.IsExecutionCompleted);
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
                TimesThisStepWasExecuted++;
                TotalTimeSpent += timer.Elapsed;
                timer.Stop();
            }
        }

        public void Print()
        {
            TestExecutionStepPrinter.Print(this);

            _children.ForEach(c => c.Print());
        }

        public void PrintOnlyErrors()
        {
            if (!this.ThisBranchHadAnExecutionError)
                return;

            TestExecutionStepPrinter.Print(this);

            _children.ForEach(c => c.PrintOnlyErrors());
        }
    }
}