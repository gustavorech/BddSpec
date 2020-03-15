using System;
using BddSpec.Execution;
using BddSpec.Options;
using BddSpec.Printer;

namespace BddSpec
{
    public class BddSpecManager
    {
        public bool ShouldExitWithCode { get; set; }

        public BddSpecManager(bool shouldExitWithCode = true)
        {
            ShouldExitWithCode = shouldExitWithCode;
        }

        public void Execute(params string[] args)
        {
            try
            {
                OptionsReader optionsReader = new OptionsReader();

                bool successReadingOptions =
                    optionsReader.CreateExecuteConfigurationFromOptions(args);

                if (!successReadingOptions)
                {
                    VerifyIfShouldExitWith(1);
                    return;
                }

                SuiteExecutor suiteExecutor = new SuiteExecutor();

                bool successExecuting = suiteExecutor.DiscoverAndExecute();

                if (!successExecuting)
                {
                    VerifyIfShouldExitWith(1);
                    return;
                }

                VerifyIfShouldExitWith(0);
            }
            catch (Exception ex)
            {
                ExecutionPrinter.PrintUnknownFatalError(ex);
                VerifyIfShouldExitWith(1);
            }
        }

        private void VerifyIfShouldExitWith(int statusCode)
        {
            if (ShouldExitWithCode)
                Environment.Exit(statusCode);
        }
    }
}