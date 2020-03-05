using System;
using bddlike;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BddSpec.ConsoleTest
{
	

	class Program
	{
		static void Main(string[] args)
		{
			TestExecutor<ToTestTest> testExecutor = new TestExecutor<ToTestTest>();
			testExecutor.Execute();
			testExecutor.Print();

			Console.WriteLine();
			Console.WriteLine();

			TestExecutor<ToTestInjection> testExecutor2 = new TestExecutor<ToTestInjection>();
			testExecutor2.Execute();
			testExecutor2.Print();

			Console.ReadLine();
		}
	}
}
