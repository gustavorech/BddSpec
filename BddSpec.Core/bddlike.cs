using System;
using System.Collections.Generic;

namespace bddlike
{

	public enum TestContextType
	{
		Before,
		After,
		Method,
		Context,
		It
	}

	public class TestContext
	{
		public string Description { get; }
		public TestContextType Type { get; }
		public Action Action { get; }

		public TestContext(string description, TestContextType type, Action action)
		{
			Description = description;
			Type = type;
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

			public Action Do
			{
				set
				{
					bddLike.testContexts.Add(new TestContext(description, type, value));
				}
			}

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

		public Action ConfigureContext
		{
			set
			{
				testContexts.Add(new TestContext(null, TestContextType.Before, value));
			}
		}

		public void When(string description, Action action) =>
			testContexts.Add(new TestContext(description, TestContextType.Context, action));

		public void It(string description, Action action) =>
			testContexts.Add(new TestContext(description, TestContextType.It, action));
	}
}
