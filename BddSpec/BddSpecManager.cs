using System;
using BddSpec.Execution;
using BddSpec.Options;

namespace BddSpec
{
    public class BddSpecManager
    {
        public static void Execute(string[] args)
        {
            bool successReadingOptions =
                OptionsProcessor.CreateExecuteConfigurationFromOptions(args);

            if (!successReadingOptions)
                Environment.Exit(1);

            bool successExecuting = SuiteExecutor.DiscoverAndExecute();

            if (!successExecuting)
                Environment.Exit(1);

            Environment.Exit(0);
        }
    }
}