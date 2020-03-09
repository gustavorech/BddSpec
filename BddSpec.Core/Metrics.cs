using System;

namespace BddSpec.Core
{
    internal class Metrics
    {
        internal int TotalTestClasses { get; set; }
        internal int TotalNodesReached { get; set; }
        internal int TotalNodesExecuted { get; set; }
        internal int TotalLeafNodes { get; set; }
        internal int TotalLeafNodesPassed { get; set; }
        internal int TotalNodeErrors { get; set; }
        internal TimeSpan TotalTime { get; set; }
    }
}