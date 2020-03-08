using System;
using BddSpec.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BddSpec.ConsoleTest
{
    public class ToTest
    {
        public string Name { get; set; }
    }

    public class ToTestTest : SpecClass
    {
        public override void SetUpSpecs()
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
                    It("first letter is G", () => Assert.AreEqual(toTest.Name[0], 'G')); ;
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
