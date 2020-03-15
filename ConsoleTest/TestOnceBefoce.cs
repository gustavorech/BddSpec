using System;
using BddSpec;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTest
{
    public class TestOnceBefore : SpecClass
    {
        static string name;

        public override void SetupSpecs()
        {
            When("context", () =>
            {
                It("inner1.1", () =>
                {
                    OnceBefore = () => throw new Exception();

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
                        name.Should().Be("marcelo");
                    });
                });
                It("inner1.3", () =>
                {
                    After = () => name.Should().Be("rodrigo");
                });
            });
        }
    }
}
