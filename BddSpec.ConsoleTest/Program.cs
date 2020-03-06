using System;
using bddlike;

namespace BddSpec.ConsoleTest
{
	class Program
	{
		static void Main(string[] args)
		{
			TimeSpan.FromSeconds(0).ToString("ss.ffffff");

			Reflection.GetAllEntities();

			Console.ReadLine();
		}
	}
}
