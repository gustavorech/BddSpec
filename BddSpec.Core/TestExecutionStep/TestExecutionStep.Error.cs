
using System;

namespace BddSpec.Core
{
    public partial class TestExecutionStep
    {
        private bool _innerStepHadError;
        private bool _hadError;

        public Exception ErrorException { get; private set; }

        public bool IsInnerStepHadExecutionError { get => _innerStepHadError; }
        public bool IsHadAnExecutionError { get => _hadError; }
        public bool IsBranchHadAnExecutionError { get => IsInnerStepHadExecutionError || IsHadAnExecutionError; }

        private void NotifyHadAnExecutionError(Exception ex)
        {
            ErrorException = ex;
            _hadError = true;

            _parentExecutionStep?.NotifyInnerStepHadAnExecutionError();
        }

        private void NotifyInnerStepHadAnExecutionError()
        {
            if (_innerStepHadError)
                return;

            _innerStepHadError = true;
            _parentExecutionStep?.NotifyInnerStepHadAnExecutionError();
        }
    }
}