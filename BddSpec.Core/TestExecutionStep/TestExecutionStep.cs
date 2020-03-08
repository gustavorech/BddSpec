
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    public partial class TestExecutionStep
    {
        private TestExecutionStep _parentStep;
        private List<TestExecutionStep> _innerSteps = new List<TestExecutionStep>();
        public int PositionToGetTheActionInTheStack { get; }
        public int StepLevel { get; }
        public TestStepDescription TestStepDescription { get; }

        public int TimesThisStepWasExecuted { get; private set; }
        public TimeSpan TotalTimeSpent { get; private set; } = TimeSpan.Zero;
        public bool IsALeafStep { get => _innerSteps.Count == 0; }

        public TestExecutionStep(TestExecutionStep parentStep,
            TestStepAction stepAction, int positionInStack, int level)
        {
            this._parentStep = parentStep;
            TestStepDescription = stepAction.Description;
            PositionToGetTheActionInTheStack = positionInStack;
            StepLevel = level;
        }

        public void CreateInnerStepFromAction(
            TestStepAction stepAction, int positionThisActionWillBeFindOnTheStack)
        {
            TestExecutionStep innerExecutionStep = new TestExecutionStep(this, stepAction,
                positionThisActionWillBeFindOnTheStack, StepLevel + 1);

            _innerSteps.Add(innerExecutionStep);
        }

        public TestExecutionStep GetNextStepToExecute()
        {
            return _innerSteps.FirstOrDefault(c => !c.IsCompleted);
        }

        public void Print()
        {
            TestExecutionStepPrinter.Print(this);

            _innerSteps.ForEach(c => c.Print());
        }

        public void PrintOnlyErrors()
        {
            if (!this.IsBranchHadError)
                return;

            TestExecutionStepPrinter.Print(this);

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

            metrics.TotalNodesExecuted += TimesThisStepWasExecuted;
            metrics.TotalTime += TotalTimeSpent;

            _innerSteps.ForEach(step => step.CollectMetrics(metrics));
        }
    }
}