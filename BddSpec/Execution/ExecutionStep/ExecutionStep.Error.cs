
using System;
using BddSpec.Printer;

namespace BddSpec.Execution
{
    public partial class ExecutionStep
    {
        private bool _isInnerStepHadError;
        private bool _isHadError;

        public Exception ErrorException { get; private set; }

        public bool IsInnerStepHadError { get => _isInnerStepHadError; }
        public bool IsHadError { get => _isHadError; }
        public bool IsBranchHadError { get => IsInnerStepHadError || IsHadError; }

        private void NotifyHadError(Exception ex)
        {
            if (_isHadError)
                return;

            _isHadError = true;
            ErrorException = ex;

            ExecutionPrinter.NotifyError(this);

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