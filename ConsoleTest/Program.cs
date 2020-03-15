using System;
using BddSpec;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            new BddSpecManager().Execute(args);
        }
    }
}
