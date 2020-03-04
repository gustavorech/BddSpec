using System;
using System.Collections.Generic;
using System.Linq;

namespace bddlike
{
	public class TestExecutorDescription
	{
		public List<TestExecutorDescription> Children { get; } = new List<TestExecutorDescription>();
		public int PositionInStack { get; }
		public int Level { get; }
		public string Description { get; }
		public TestContextType Type { get; }

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

		public TestExecutorDescription(TestContext context, int positionInStack, int level)
		{
			Description = context.Description;
			Type = context.Type;
			PositionInStack = positionInStack;
			Level = level;
		}

		public void AddChild(TestContext context, int positionInStack)
		{
			Children.Add(new TestExecutorDescription(context, positionInStack, Level + 1));
		}

		public void Print()
		{
			if (Level > 0)
				Console.Write(new string(' ', Level * 2));

			if (BranchHasExecutionError)
				Console.ForegroundColor = ConsoleColor.DarkRed;
			else
				Console.ForegroundColor = ConsoleColor.Green;

			switch (Type)
			{
				case TestContextType.Context:
					Console.Write("[When] ");
					break;
				case TestContextType.It:
					Console.Write("[It] ");
					break;
				default:
					break;
			}

			Console.Write(Description);
			Console.WriteLine();

			Children.ForEach(c => c.Print());
		}
	}

	public class TestExecutor<T>
		where T : BddLike, new()
	{
		private int executed = 0;
		private List<TestExecutorDescription> root = new List<TestExecutorDescription>();

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
				T instance = new T();
				instance.ConfigureTests();

				Recursion(instance, root);
			}
		}

		private void Recursion(T instance, List<TestExecutorDescription> listToExecute)
		{
			TestExecutorDescription currentTestExecutor = listToExecute.First(c => !c.IsExecutionCompleted);
			TestContext currentTestContext = instance.testContexts[currentTestExecutor.PositionInStack];

			int currentStackCount = instance.testContexts.Count;
			SafeInvoke(currentTestContext, currentTestExecutor);

			if (currentTestExecutor.HasExecutionError)
				return;

			if (!currentTestExecutor.IsChildrenDiscovered)
			{
				for (int i = currentStackCount; i < instance.testContexts.Count; i++)
				{
					TestContext childTestContext = instance.testContexts[i];
					currentTestExecutor.AddChild(childTestContext, i);
				}

				currentTestExecutor.IsChildrenDiscovered = true;

				if (currentTestExecutor.IsExecutionCompleted)
					return;
			}

			Recursion(instance, currentTestExecutor.Children);
		}

		private void Initialize()
		{
			T instance = new T();

			instance.ConfigureTests();

			for (int i = executed; i < instance.testContexts.Count; i++)
			{
				TestContext testContext = instance.testContexts[i];
				root.Add(new TestExecutorDescription(testContext, i, 1));
			}
		}

		private void SafeInvoke(TestContext context, TestExecutorDescription executorDescription)
		{
			try
			{
				context.Action.Invoke();
			}
			catch
			{
				executorDescription.HasExecutionError = true;
			}
		}

		public void Print()
		{
			Console.Write(typeof(T).Name + ":");
			Console.WriteLine();

			root.ForEach(c => c.Print());
		}
	}
}
