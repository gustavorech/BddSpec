using System;
using BddSpec.Core;
using BddSpec.Core.Options;

namespace BddSpec.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!OptionsProcessor.ReadOptions(args))
                Environment.Exit(1);

            TestDiscoverer.DiscoverAndExecute();

            Console.ReadLine();
        }
    }
}
