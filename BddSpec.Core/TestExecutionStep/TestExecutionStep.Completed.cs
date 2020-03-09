
using BddSpec.Core.Printer;

namespace BddSpec.Core
{
    public partial class TestExecutionStep
    {
        private bool _isCompleted;
        private int _quantityOfInnerStepsCompleted;

        public bool IsCompleted { get => _isCompleted; }

        public void NotifyCompleted()
        {
            if (_isCompleted)
                return;

            _isCompleted = true;

            _parentStep?.NotifyInnerStepCompleted();
        }

        public void NotifyInnerStepCompleted()
        {
            _quantityOfInnerStepsCompleted++;

            if (_quantityOfInnerStepsCompleted == _innerSteps.Count)
                NotifyCompleted();
        }
    }
}