using System;
using BddSpec;

namespace BddSpec.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            BddSpecManager.Execute(args);

            Environment.Exit(0);
        }
    }
}
