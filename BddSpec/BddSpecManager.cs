using System;
using BddSpec.Execution;
using BddSpec.Options;

namespace BddSpec
{
    public class BddSpecManager
    {
        public static void Execute(string[] args)
        {
            bool successReadOptions =
                OptionsProcessor.CreateExecuteConfigurationFromOptions(args);

            if (!successReadOptions)
                Environment.Exit(1);

            bool success = SuiteExecutor.DiscoverAndExecute();

            if (!success)
                Environment.Exit(1);

            Environment.Exit(0);
        }
    }
}