
using System;

namespace bddlike
{
    public partial class TestExecutionStep
    {
        private bool _childHadError;
        private bool _hadError;

        public bool AnyChildHadExecutionError { get => _childHadError; }
        public bool ThisStepHadAnExecutionError { get => _hadError; }
        public bool ThisBranchHadAnExecutionError { get => AnyChildHadExecutionError || ThisStepHadAnExecutionError; }

        private void NotifyHadAnExecutionError()
        {
            _hadError = true;

            _parent?.NotifyChildHadAnExecutionError();
        }

        private void NotifyChildHadAnExecutionError()
        {
            if (_childHadError)
                return;

            _childHadError = true;
            _parent?.NotifyChildHadAnExecutionError();
        }
    }
}