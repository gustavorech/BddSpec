using System;
using BddSpec.Core.Options;

namespace BddSpec.Core
{
    public class SpecExecutor
    {
        public static void Execute(string[] args)
        {
            if (!OptionsProcessor.ReadOptions(args))
                Environment.Exit(1);

            TestDiscoverer.DiscoverAndExecute();
        }
    }
}