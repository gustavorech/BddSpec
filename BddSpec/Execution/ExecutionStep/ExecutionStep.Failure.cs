
using System;
using BddSpec.Printer;

namespace BddSpec.Execution
{
    public partial class ExecutionStep
    {
        private bool _isAnyInnerStepFailed;
        private bool _isFailed;

        public Exception ErrorException { get; private set; }

        public bool IsAnyInnerStepFailed { get => _isAnyInnerStepFailed; }
        public bool IsFailed { get => _isFailed; }
        public bool IsBAnyInBranchFailed { get => IsAnyInnerStepFailed || IsFailed; }

        public void NotifyFailure(Exception exception)
        {
            if (_isFailed)
                return;

            _isFailed = true;
            ErrorException = exception;

            VerbosePrinter.NotifyError(this);

            NotifyCompletion();

            _parentStep?.NotifyInnerStepFailure();
        }

        private void NotifyInnerStepFailure()
        {
            if (_isAnyInnerStepFailed)
                return;

            _isAnyInnerStepFailed = true;
            _parentStep?.NotifyInnerStepFailure();
        }
    }
}