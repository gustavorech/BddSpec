using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace bddlike
{
	public class TestExecutionStep
	{
		public TestExecutionStep Parent { get; }
		public List<TestExecutionStep> Children { get; } = new List<TestExecutionStep>();
		public int PositionInStack { get; }
		public int StepLevel { get; }
		public TestContextDescription TestContextDescription { get; }

		public int ExecutionTimes { get; private set; }
		public TimeSpan TimeSpent { get; private set; } = TimeSpan.Zero;
		public bool IsChildrenDiscovered { get; set; }
		public bool HasExecutionError { get; set; }

		public bool IsExecutionCompleted
		{
			get
			{
				if (HasExecutionError)
					return true;

				return IsChildrenDiscovered && Children.TrueForAll(c => c.IsExecutionCompleted);
			}
		}

		public bool BranchHasExecutionError
		{
			get
			{
				return HasExecutionError || Children.Any(c => c.BranchHasExecutionError);
			}
		}

		public TestExecutionStep(TestExecutionStep parent, TestContext context, int positionInStack, int level)
		{
			Parent = parent;
			TestContextDescription = context.Description;
			PositionInStack = positionInStack;
			StepLevel = level;
		}

		public void AddChild(TestContext context, int positionInStack)
		{
			Children.Add(new TestExecutionStep(this, context, positionInStack, StepLevel + 1));
		}

		public void SafeInvoke(TestContext context)
		{
			Stopwatch timer = Stopwatch.StartNew();

			try
			{
				context.Action.Invoke();
			}
			catch
			{
				HasExecutionError = true;
			}
			finally
			{
				ExecutionTimes++;
				TimeSpent += timer.Elapsed;
				timer.Stop();
			}
		}

		public void Print()
		{
			Console.Write(new string(' ', StepLevel * 2));

			if (HasExecutionError)
				Console.ForegroundColor = ConsoleColor.DarkRed;
			else if (Children.Count == 0)
				Console.ForegroundColor = ConsoleColor.Green;
			else
				Console.ResetColor();

			switch (TestContextDescription.ContextType)
			{
				case TestContextType.Method:
					Console.Write("[");
					break;
				case TestContextType.When:
					Console.Write("-when ");
					break;
				case TestContextType.It:
					Console.Write(".it ");
					break;
				default:
					break;
			}

			Console.Write(TestContextDescription.TestDescription);

			if (TestContextDescription.ContextType == TestContextType.Method)
				Console.Write(" method]:");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write($" (ln:{TestContextDescription.SourceFileNumber})");

			//if (HasExecutionError)
			//	Console.Write(" --FAIL--");

			PrintTimeStamp();

			Console.WriteLine();

			Children.ForEach(c => c.Print());
		}

		public void PrintTimeStamp()
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;

			if (TimeSpent > TimeSpan.FromMinutes(1))
				Console.Write(" (" + TimeSpent.ToString("mm:ss") + "minutes)");
			else if (TimeSpent > TimeSpan.FromSeconds(1))
				Console.Write(" (" + TimeSpent.ToString("ss.fff") + "s)");
			else if (TimeSpent > TimeSpan.FromMilliseconds(100))
				Console.Write(" (" + TimeSpent.ToString("fff") + "ms)");
			else if (TimeSpent > TimeSpan.FromMilliseconds(10))
				Console.Write(" (" + TimeSpent.ToString("fff").Substring(1) + "ms)");
		}
	}

	public class TestExecutor
	{
		private Type type;
		private int executed = 0;
		private List<TestExecutionStep> root = new List<TestExecutionStep>();

		public TestExecutor(Type type)
		{
			this.type = type;
		}

		public void Execute()
		{
			Initialize();

			if (root.Count == 0)
				return;

			Iterate();
		}

		private void Iterate()
		{
			while (!root.TrueForAll(c => c.IsExecutionCompleted))
			{
				BddLike instance = (BddLike)Activator.CreateInstance(type);
				instance.ConfigureTests();

				Recursion(instance, root);
			}
		}

		private void Recursion(BddLike instance, List<TestExecutionStep> listToExecute)
		{
			TestExecutionStep currentTestExecutor = listToExecute.First(c => !c.IsExecutionCompleted);
			TestContext currentTestContext = instance.testContexts[currentTestExecutor.PositionInStack];

			int currentStackCount = instance.testContexts.Count;
			currentTestExecutor.SafeInvoke(currentTestContext);

			if (currentTestExecutor.HasExecutionError)
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write("F");
				return;
			}

			if (!currentTestExecutor.IsChildrenDiscovered)
			{
				for (int i = currentStackCount; i < instance.testContexts.Count; i++)
				{
					TestContext childTestContext = instance.testContexts[i];
					currentTestExecutor.AddChild(childTestContext, i);
				}

				currentTestExecutor.IsChildrenDiscovered = true;

				if (currentTestExecutor.IsExecutionCompleted)
				{
					if (currentTestExecutor.BranchHasExecutionError)
					{
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.Write("F");
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.Write(".");
					}

					return;
				}
			}

			Recursion(instance, currentTestExecutor.Children);
		}

		private void Initialize()
		{
			BddLike instance = (BddLike)Activator.CreateInstance(type);

			instance.ConfigureTests();

			for (int i = executed; i < instance.testContexts.Count; i++)
			{
				TestContext testContext = instance.testContexts[i];
				root.Add(new TestExecutionStep(null, testContext, i, 1));
			}
		}

		public void Print()
		{
			Console.ResetColor();
			Console.WriteLine();
			Console.WriteLine(root.First().TestContextDescription.SourceFilePath);
			Console.WriteLine("class: " + type.Name);

			root.ForEach(c => c.Print());
		}
	}
}
