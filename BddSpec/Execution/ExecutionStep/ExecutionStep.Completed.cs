
using BddSpec.Printer;

namespace BddSpec.Execution
{
    public partial class ExecutionStep
    {
        private bool _isInitialized;
        private bool _isCompleted;
        private int _quantityOfInnerStepsCompleted;

        public bool IsInitialized { get => _isInitialized; }
        public bool IsCompleted { get => _isCompleted; }

        private void NotifyInitialized()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            ExecutionPrinter.NotifyInitialized(this);
        }

        public void NotifyCompletion()
        {
            if (_isCompleted)
                return;

            _isCompleted = true;

            ExecutionPrinter.NotifyCompleted(this);

            _parentStep?.NotifyInnerStepCompletion();
        }

        public void NotifyInnerStepCompletion()
        {
            _quantityOfInnerStepsCompleted++;

            if (_quantityOfInnerStepsCompleted == _innerSteps.Count)
                NotifyCompletion();
        }
    }
}