﻿using System;
using System.Diagnostics;
using System.Linq;

namespace bddlike
{
	public class Reflection
	{
        public static void GetAllEntities()
        {
			Stopwatch timer = Stopwatch.StartNew();

			AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
				 .Where(x => typeof(BddLike).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
				 .ToList()
				 .ForEach(type =>
				 {
					 TestExecutor testExecutor = new TestExecutor(type);
					 testExecutor.Execute();
					 testExecutor.Print();
				 });

				 Console.WriteLine("ACABEI: " + timer.Elapsed.ToString());
        }
    }
}
