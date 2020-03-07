using System;

namespace BddSpec.Core
{
    public class TestStepAction
    {
        public TestStepDescription Description { get; }
        public Action Action { get; }

        public TestStepAction(TestStepDescription description, Action action)
        {
            Description = description;
            Action = action;
        }
    }
}
