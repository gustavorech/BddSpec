using System;
using BddSpec.Core;

namespace BddSpec.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestDiscoverer.DiscoverAndExecute();

            Console.ReadLine();
        }
    }
}
