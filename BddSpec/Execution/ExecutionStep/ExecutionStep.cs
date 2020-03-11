
using System;
using BddSpec.Configuration;
using BddSpec.Printer;

namespace BddSpec.Execution
{
    public partial class ExecutionStep
    {
        private ExecutionStep _parentStep;
        public int PositionOfTheActionInTheStack { get; }
        public int StepLevel { get; }
        public SpecDescription TestStepDescription { get; }

        public int TimesExecuted { get; private set; }
        public TimeSpan TotalTimeSpent { get; private set; } = TimeSpan.Zero;

        public ExecutionStep(ExecutionStep parentStep,
            SpecAction stepAction, int positionInStack, int level)
        {
            this._parentStep = parentStep;
            TestStepDescription = stepAction.Description;
            PositionOfTheActionInTheStack = positionInStack;
            StepLevel = level;
        }

        public void PrintSummary()
        {
            TestExecutionStepPrinter.PrintVerbose(this);

            _innerSteps.ForEach(c => c.PrintSummary());
        }

        public void PrintErrorsDetailed()
        {
            if (!this.IsBranchHadError)
                return;

            TestExecutionStepPrinter.PrintVerbose(this);

            if (IsHadError)
                ExceptionPrinter.Print(ErrorException);

            _innerSteps.ForEach(c => c.PrintErrorsDetailed());
        }

        public void PrintErrorsSummary()
        {
            if (!this.IsBranchHadError)
                return;

            TestExecutionStepPrinter.PrintVerbose(this);

            _innerSteps.ForEach(c => c.PrintErrorsSummary());
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