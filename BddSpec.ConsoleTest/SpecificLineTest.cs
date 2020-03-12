using System;
using BddSpec;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BddSpec.ConsoleTest
{
    public class SpecificLineTest : SpecClass
    {
        public override void SetUpSpecs()
        {
            ToTest toTest = new ToTest();

            When("context", () =>
            {
                It("inner1.1", () =>
                {
                    It("inner2.1", () =>
                    {

                    });
                    It("inner2.2", () =>
                    {

                    });
                });
                It("inner1.2", () =>
                {
                    It("inner2.3", () =>
                    {

                    });
                    It("inner2.4", () =>
                    {

                    });
                });
                It("inner1.3", () =>
                {
                });
            });
        }
    }
}
