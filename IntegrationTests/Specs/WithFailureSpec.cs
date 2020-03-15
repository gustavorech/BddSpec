using BddSpec;
using FluentAssertions;

namespace IntegrationTests
{
    public class WithFailureSpec : SpecClass
    {
        public override void SetupSpecs()
        {
            Context("context_1", () =>
            {
                When("when_1", () =>
                {
                    It("it_1", () =>
                    {

                    });

                    It("it_2", () =>
                    {
                        false.Should().Be(true);
                    });
                });

                When("when_2", () =>
                {
                    false.Should().Be(true);

                    It("it_3", () =>
                    {

                    });
                });

                When("when_3", () =>
                {

                });
            });
        }
    }
}
