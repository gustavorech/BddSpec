using System;

namespace BddSpec
{
    public class SpecAction
    {
        public SpecDescription Description { get; }
        public Action Action { get; }

        public SpecAction(SpecDescription description, Action action)
        {
            Description = description;
            Action = action;
        }
    }
}
