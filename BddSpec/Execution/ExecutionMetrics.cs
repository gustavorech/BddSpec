using System;

namespace BddSpec.Execution
{
    public class ExecutionMetrics
    {
        public int TotalTestClasses { get; set; }
        public int TotalNodesReached { get; set; }
        public int TotalNodesExecuted { get; set; }
        public int TotalLeafNodes { get; set; }
        public int TotalLeafNodesPassed { get; set; }
        public int TotalNodeErrors { get; set; }
        public TimeSpan TotalTime { get; set; }
    }
}