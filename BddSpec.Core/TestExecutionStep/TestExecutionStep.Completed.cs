
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    internal partial class TestExecutionStep
    {
        private bool _isInitialized;
        private bool _isCompleted;
        private int _quantityOfInnerStepsCompleted;

        internal bool IsInitialized { get => _isInitialized; }
        internal bool IsCompleted { get => _isCompleted; }

        private void NotifyInitialized()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;
        }

        internal void NotifyCompleted()
        {
            if (_isCompleted)
                return;

            _isCompleted = true;

            _parentStep?.NotifyInnerStepCompleted();
        }

        internal void NotifyInnerStepCompleted()
        {
            _quantityOfInnerStepsCompleted++;

            if (_quantityOfInnerStepsCompleted == _innerSteps.Count)
                NotifyCompleted();
        }
    }
}