using System;
using BddSpec;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BddSpec.ConsoleTest.OtherNamespace
{
    public class OneToTestTest : SpecClass
    {
        public override void SetupSpecs()
        {
            ToTest toTest = new ToTest();
            toTest.Name = "Gustavo";

            When("the name is Gustavo", () =>
            {
                It("is Gustavo", () => Assert.AreEqual(toTest.Name, "Gustavo"));
            });
        }
    }
    public class ToTestTest2 : SpecClass
    {
        public override void SetupSpecs()
        {
            ToTest toTest = new ToTest();
            toTest.Name = "Gustavo";

            When("the name is Gustavo", () =>
            {
                It("is Gustavo", () => Assert.AreEqual(toTest.Name, "Gustavo"));
            });
        }
    }

    public class ToTestTest3 : SpecClass
    {
        public override void SetupSpecs()
        {
            ToTest toTest = new ToTest();
            toTest.Name = "Gustavo";

            When("the name is Gustavo", () =>
            {
                It("is Gustavo", () => Assert.AreEqual(toTest.Name, "Gustavo"));
            });
        }
    }
}
