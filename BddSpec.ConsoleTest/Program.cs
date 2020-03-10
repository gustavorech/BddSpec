using System;
using BddSpec.Core;

namespace BddSpec.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SpecExecutor.Execute(args);

            Environment.Exit(0);
        }
    }
}
