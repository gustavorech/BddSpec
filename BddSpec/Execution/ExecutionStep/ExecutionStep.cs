
using System;
using BddSpec.Printer;

namespace BddSpec.Execution
{
    internal partial class ExecutionStep
    {
        private ExecutionStep _parentStep;
        internal int PositionToGetTheActionInTheStack { get; }
        internal int StepLevel { get; }
        internal SpecDescription TestStepDescription { get; }

        internal int TimesExecuted { get; private set; }
        internal TimeSpan TotalTimeSpent { get; private set; } = TimeSpan.Zero;

        internal ExecutionStep(ExecutionStep parentStep,
            SpecAction stepAction, int positionInStack, int level)
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

        internal void CollectMetrics(ExecutionMetrics metrics)
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