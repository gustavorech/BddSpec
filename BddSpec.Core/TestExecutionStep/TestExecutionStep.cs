
using System;
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    public partial class TestExecutionStep
    {
        private TestExecutionStep _parentStep;
        public int PositionToGetTheActionInTheStack { get; }
        public int StepLevel { get; }
        public TestStepDescription TestStepDescription { get; }

        public int TimesExecuted { get; private set; }
        public TimeSpan TotalTimeSpent { get; private set; } = TimeSpan.Zero;

        public TestExecutionStep(TestExecutionStep parentStep,
            TestStepAction stepAction, int positionInStack, int level)
        {
            this._parentStep = parentStep;
            TestStepDescription = stepAction.Description;
            PositionToGetTheActionInTheStack = positionInStack;
            StepLevel = level;
        }

        public void Print()
        {
            TestExecutionStepPrinter.PrintVerbose(this);

            _innerSteps.ForEach(c => c.Print());
        }

        public void PrintOnlyErrors()
        {
            if (!this.IsBranchHadError)
                return;

            TestExecutionStepPrinter.PrintVerbose(this);

            _innerSteps.ForEach(c => c.PrintOnlyErrors());
        }

        public void CollectMetrics(Metrics metrics)
        {
            metrics.TotalNodesReached++;

            if (IsALeafStep)
                metrics.TotalLeafNodes++;

            if (IsHadError)
                metrics.TotalNodeErrors++;
            else if (IsALeafStep)
                metrics.TotalLeafNodesPassed++;

            metrics.TotalNodesExecuted += TimesExecuted;
            metrics.TotalTime += TotalTimeSpent;

            _innerSteps.ForEach(step => step.CollectMetrics(metrics));
        }
    }
}