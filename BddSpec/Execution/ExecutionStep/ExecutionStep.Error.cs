
using System;

namespace BddSpec.Execution
{
    internal partial class ExecutionStep
    {
        private bool _isInnerStepHadError;
        private bool _isHadError;

        internal Exception ErrorException { get; private set; }

        internal bool IsInnerStepHadError { get => _isInnerStepHadError; }
        internal bool IsHadError { get => _isHadError; }
        internal bool IsBranchHadError { get => IsInnerStepHadError || IsHadError; }

        private void NotifyHadError(Exception ex)
        {
            if (_isHadError)
                return;

            _isHadError = true;
            ErrorException = ex;

            NotifyCompleted();

            _parentStep?.NotifyInnerStepHadError();
        }

        private void NotifyInnerStepHadError()
        {
            if (_isInnerStepHadError)
                return;

            _isInnerStepHadError = true;
            _parentStep?.NotifyInnerStepHadError();
        }
    }
}