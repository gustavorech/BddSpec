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

            SuiteExecutor.DiscoverAndExecute();
        }
    }
}