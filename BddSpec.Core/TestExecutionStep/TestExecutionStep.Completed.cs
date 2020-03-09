
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    internal partial class TestExecutionStep
    {
        private bool _isCompleted;
        private int _quantityOfInnerStepsCompleted;

        internal bool IsCompleted { get => _isCompleted; }

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