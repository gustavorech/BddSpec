using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

	public class ToTest
	{
		public string Name { get; set; }
	}

	public class ToTestTest : BddLike
	{
		public override void ConfigureTests()
		{
			ToTest toTest = new ToTest();

			When("the name is Gustavo", () =>
			{
				toTest.Name = "Gustavo";

				It("is Gustavo", () => Assert.AreEqual(toTest.Name, "Gustavo"));
				It("first letter is G", () => Assert.AreEqual(toTest.Name[0], 'P'));

				When("has surname Rech", () =>
				{
					toTest.Name += " Rech";

					throw new Exception("oq aconteceu");

					It("is Gustavo Rech", () => Assert.AreEqual(toTest.Name, "Gustavo Rech"));
					It("first letter is G", () => Assert.AreEqual(toTest.Name[0], 'G'));;
				});
			});

			When("the name is Pedro", () =>
			{
				toTest.Name = "Pedro";

				It("is Pedro", () => Assert.AreEqual(toTest.Name, "Pedro"));
				It("first letter is P", () => Assert.AreEqual(toTest.Name[0], 'P'));
			});
		}
	}
}
