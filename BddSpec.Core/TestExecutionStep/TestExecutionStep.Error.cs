
using System;

namespace BddSpec.Core
{
    public partial class TestExecutionStep
    {
        private bool _childHadError;
        private bool _hadError;

        public bool AnyChildHadExecutionError { get => _childHadError; }
        public bool HadAnExecutionError { get => _hadError; }
        public bool ThisBranchHadAnExecutionError { get => AnyChildHadExecutionError || HadAnExecutionError; }

        private void NotifyHadAnExecutionError()
        {
            _hadError = true;

            _parentExecutionStep?.NotifyChildHadAnExecutionError();
        }

        private void NotifyChildHadAnExecutionError()
        {
            if (_childHadError)
                return;

            _childHadError = true;
            _parentExecutionStep?.NotifyChildHadAnExecutionError();
        }
    }
}