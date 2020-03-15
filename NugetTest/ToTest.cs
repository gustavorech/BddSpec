using FluentAssertions;

namespace NugetTest
{
    public class ToTest
    {
        public string Name { get; set; }
    }

    public class ToTestTest : SpecClass
    {
        public override void SetupSpecs()
        {
            ToTest toTest = new ToTest();

            When("the name is Gustavo", () =>
            {
                toTest.Name = "Gustavo";

                It("is Gustavo", () => toTest.Name.Should().Be("Gustavo"));
                It("first letter is G", () => toTest.Name[0].Should().Be('G'));

                When("has surname Rech", () =>
                {
                    toTest.Name += " Rech";

                    //throw new Exception("oq aconteceu");

                    It("is Gustavo Rech", () => toTest.Name.Should().Be("Gustavo Rech"));
                    It("first letter is G", () => toTest.Name[0].Should().Be('G'));
                });
            });

            When("the name is Pedro", () =>
            {
                //After = () => throw new Exception("wtf is going on?");
                toTest.Name = "Pedro";

                It("is Pedro", () => toTest.Name.Should().Be("Pedro"));
                It("first letter is P", () => toTest.Name[0].Should().Be('P'));
            });
        }
    }
}
