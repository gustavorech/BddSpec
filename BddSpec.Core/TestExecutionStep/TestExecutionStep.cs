
using System;
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    internal partial class TestExecutionStep
    {
        private TestExecutionStep _parentStep;
        internal int PositionToGetTheActionInTheStack { get; }
        internal int StepLevel { get; }
        internal TestStepDescription TestStepDescription { get; }

        internal int TimesExecuted { get; private set; }
        internal TimeSpan TotalTimeSpent { get; private set; } = TimeSpan.Zero;

        internal TestExecutionStep(TestExecutionStep parentStep,
            TestStepAction stepAction, int positionInStack, int level)
        {
            this._parentStep = parentStep;
            TestStepDescription = stepAction.Description;
            PositionToGetTheActionInTheStack = positionInStack;
            StepLevel = level;
        }

        internal void Print()
        {
            TestExecutionStepPrinter.PrintVerbose(this);

            _innerSteps.ForEach(c => c.Print());
        }

        internal void PrintOnlyErrors()
        {
            if (!this.IsBranchHadError)
                return;

            TestExecutionStepPrinter.PrintVerbose(this);

            _innerSteps.ForEach(c => c.PrintOnlyErrors());
        }

        internal void CollectMetrics(Metrics metrics)
        {
            metrics.TotalNodesReached++;

            if (IsLeaf)
                metrics.TotalLeafNodes++;

            if (IsHadError)
                metrics.TotalNodeErrors++;
            else if (IsLeaf)
                metrics.TotalLeafNodesPassed++;

            metrics.TotalNodesExecuted += TimesExecuted;
            metrics.TotalTime += TotalTimeSpent;

            _innerSteps.ForEach(step => step.CollectMetrics(metrics));
        }
    }
}