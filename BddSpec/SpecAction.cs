using System;

namespace BddSpec
{
    internal class SpecAction
    {
        internal SpecDescription Description { get; }
        internal Action Action { get; }

        internal SpecAction(SpecDescription description, Action action)
        {
            Description = description;
            Action = action;
        }
    }
}
