using System;

namespace BddSpec.Execution
{
    public class ExecutionMetrics
    {
        public int TotalSpecClasses { get; set; }
        public int TotalNodesReached { get; set; }
        public int TotalTimesNodesWereExecuted { get; set; }
        public int TotalLeafNodesReached { get; set; }
        public int TotalLeafNodesSucceeded { get; set; }
        public int TotalNodeWithFailures { get; set; }
        public TimeSpan TotalExecutionTimeSpent { get; set; }
    }
}