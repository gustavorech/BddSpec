
using System;
using BddSpec.Configuration;
using BddSpec.Printer;

namespace BddSpec.Execution
{
    public partial class ExecutionStep
    {
        private ExecutionStep _parentStep;
        public int PositionOfTheActionOnSpecClass { get; }
        public int StepLevel { get; }
        public SpecDescription TestStepDescription { get; }

        public int TotalTimesExecuted { get; private set; }
        public TimeSpan TotalTimeSpent { get; private set; } = TimeSpan.Zero;

        public ExecutionStep(ExecutionStep parentStep,
            SpecAction stepAction, int positionInStack, int stepLevel)
        {
            _parentStep = parentStep;
            TestStepDescription = stepAction.Description;
            PositionOfTheActionOnSpecClass = positionInStack;
            StepLevel = stepLevel;
        }

        public void PrintSummary()
        {
            TestExecutionStepPrinter.PrintVerbose(this);

            _innerSteps.ForEach(c => c.PrintSummary());
        }

        public void PrintErrorsDetailed()
        {
            if (!this.IsBAnyInBranchFailed)
                return;

            TestExecutionStepPrinter.PrintVerbose(this);

            if (IsFailed)
                ExceptionPrinter.Print(ErrorException);

            _innerSteps.ForEach(c => c.PrintErrorsDetailed());
        }

        public void PrintErrorsSummary()
        {
            if (!this.IsBAnyInBranchFailed)
                return;

            TestExecutionStepPrinter.PrintVerbose(this);

            _innerSteps.ForEach(c => c.PrintErrorsSummary());
        }

        public void CollectMetrics(ExecutionMetrics metrics)
        {
            metrics.TotalNodesReached++;

            if (IsLeafStep)
                metrics.TotalLeafNodesReached++;

            if (IsFailed)
                metrics.TotalNodeWithFailures++;
            else if (IsLeafStep)
                metrics.TotalLeafNodesSucceeded++;

            metrics.TotalTimesNodesWereExecuted += TotalTimesExecuted;
            metrics.TotalExecutionTimeSpent += TotalTimeSpent;

            _innerSteps.ForEach(step => step.CollectMetrics(metrics));
        }
    }
}