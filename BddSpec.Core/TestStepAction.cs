using System;

namespace BddSpec.Core
{
    internal class TestStepAction
    {
        internal TestStepDescription Description { get; }
        internal Action Action { get; }

        internal TestStepAction(TestStepDescription description, Action action)
        {
            Description = description;
            Action = action;
        }
    }
}
