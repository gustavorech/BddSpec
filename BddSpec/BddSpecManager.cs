using System;
using BddSpec.Execution;
using BddSpec.Options;
using BddSpec.Printer;

namespace BddSpec
{
    public class BddSpecManager
    {
        public static void Execute(string[] args)
        {
            try
            {
                OptionsReader optionsReader = new OptionsReader();

                bool successReadingOptions =
                    optionsReader.CreateExecuteConfigurationFromOptions(args);

                if (!successReadingOptions)
                    Environment.Exit(1);

                SuiteExecutor suiteExecutor = new SuiteExecutor();

                bool successExecuting = suiteExecutor.DiscoverAndExecute();

                if (!successExecuting)
                    Environment.Exit(1);

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                ExecutionPrinter.PrintUnknownFatalError(ex);
                Environment.Exit(1);
            }
        }
    }
}