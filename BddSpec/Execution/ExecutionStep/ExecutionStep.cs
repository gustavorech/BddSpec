
using System;
using BddSpec.Printer;

namespace BddSpec.Execution
{
    public partial class ExecutionStep
    {
        private ExecutionStep _parentStep;
        public int PositionToGetTheActionInTheStack { get; }
        public int StepLevel { get; }
        public SpecDescription TestStepDescription { get; }

        public int TimesExecuted { get; private set; }
        public TimeSpan TotalTimeSpent { get; private set; } = TimeSpan.Zero;

        public ExecutionStep(ExecutionStep parentStep,
            SpecAction stepAction, int positionInStack, int level)
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

        public void CollectMetrics(ExecutionMetrics metrics)
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