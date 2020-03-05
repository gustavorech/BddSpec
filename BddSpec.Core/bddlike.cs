using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace bddlike
{

	public enum TestContextType
	{
		Class,
		Method,
		Before,
		After,
		Describe,
		Context,
		When,
		Then,
		It,
		Cleanup
	}

	public class TestContextDescription
	{
		public string SourceFilePath { get; }
		public int SourceFileNumber { get; }
		public string TestDescription { get; }
		public TestContextType ContextType { get; }

		public TestContextDescription(string sourceFilePath, int sourceFileNumber,
			string testDescription, TestContextType contextType)
		{
			SourceFilePath = sourceFilePath;
			SourceFileNumber = sourceFileNumber;
			TestDescription = testDescription;
			ContextType = contextType;
		}
	}

	public class TestContext
	{
		public TestContextDescription Description { get; }
		public Action Action { get; }

		public TestContext(TestContextDescription description, Action action)
		{
			Description = description;
			Action = action;
		}
	}

	public abstract partial class BddLike
	{
		public class BddLikeAction
		{
			private BddLike bddLike;
			private string description;
			private TestContextType type;

			public BddLikeAction(BddLike bddLike, string description, TestContextType type)
			{
				this.bddLike = bddLike;
				this.description = description;
				this.type = type;
			}
		}
	}

	public abstract partial class BddLike
	{
		public List<TestContext> testContexts { get; } = new List<TestContext>();

		public abstract void ConfigureTests();

		public void When(string description, Action action,
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			TestContextDescription testDescription = new TestContextDescription(
				sourceFilePath, sourceLineNumber, description, TestContextType.When);

			testContexts.Add(new TestContext(testDescription, action));
		}

		public void It(string description, Action action,
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			TestContextDescription testDescription = new TestContextDescription(
				sourceFilePath, sourceLineNumber, description, TestContextType.It);

			testContexts.Add(new TestContext(testDescription, action));
		}

		public void Method(string description, Action action,
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0)
		{
			TestContextDescription testDescription = new TestContextDescription(
				sourceFilePath, sourceLineNumber, description, TestContextType.Method);

			testContexts.Add(new TestContext(testDescription, action));
		}
	}
}
