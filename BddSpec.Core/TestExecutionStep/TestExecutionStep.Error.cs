
using System;

namespace BddSpec.Core
{
    public partial class TestExecutionStep
    {
        private bool _isInnerStepHadError;
        private bool _isHadError;

        public Exception ErrorException { get; private set; }

        public bool IsInnerStepHadError { get => _isInnerStepHadError; }
        public bool IsHadError { get => _isHadError; }
        public bool IsBranchHadError { get => IsInnerStepHadError || IsHadError; }

        private void NotifyHadError(Exception ex)
        {
            ErrorException = ex;
            _isHadError = true;

            _parentExecutionStep?.NotifyInnerStepHadError();
        }

        private void NotifyInnerStepHadError()
        {
            if (_isInnerStepHadError)
                return;

            _isInnerStepHadError = true;
            _parentExecutionStep?.NotifyInnerStepHadError();
        }
    }
}