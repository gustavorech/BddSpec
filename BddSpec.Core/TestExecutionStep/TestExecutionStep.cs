
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    public partial class TestExecutionStep
    {
        private TestExecutionStep _parentExecutionStep;
        private List<TestExecutionStep> _innerExecutionSteps = new List<TestExecutionStep>();
        public int PositionInStack { get; }
        public int StepLevel { get; }
        public TestStepDescription TestContextDescription { get; }

        public int TimesThisStepWasExecuted { get; private set; }
        public TimeSpan TotalTimeSpent { get; private set; } = TimeSpan.Zero;
        public bool InnerActionsHadBeenDiscovered { get; set; }
        public bool IsALeafStep { get => _innerExecutionSteps.Count == 0; }

        public bool IsExecutionCompleted
        {
            get
            {
                if (HadAnExecutionError)
                    return true;

                return InnerActionsHadBeenDiscovered && _innerExecutionSteps.TrueForAll(c => c.IsExecutionCompleted);
            }
        }

        public TestExecutionStep(TestExecutionStep parentExecutionStep,
            TestStepAction testStepAction, int positionInStack, int level)
        {
            this._parentExecutionStep = parentExecutionStep;
            TestContextDescription = testStepAction.Description;
            PositionInStack = positionInStack;
            StepLevel = level;
        }

        public void CreateInnerExecutionStepFromAction(TestStepAction context, int positionInStack)
        {
            _innerExecutionSteps.Add(new TestExecutionStep(this, context, positionInStack, StepLevel + 1));
        }

        public TestExecutionStep GetNextStepToExecute()
        {
            return _innerExecutionSteps.FirstOrDefault(c => !c.IsExecutionCompleted);
        }

        public void SafeInvoke(TestStepAction context)
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

            _innerExecutionSteps.ForEach(c => c.Print());
        }

        public void PrintOnlyErrors()
        {
            if (!this.ThisBranchHadAnExecutionError)
                return;

            TestExecutionStepPrinter.Print(this);

            _innerExecutionSteps.ForEach(c => c.PrintOnlyErrors());
        }
    }
}