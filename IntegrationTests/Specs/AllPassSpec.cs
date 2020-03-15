using System;
using BddSpec;

namespace IntegrationTests
{
    public class AllPassSpec : SpecClass
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

                    });
                });

                When("when_2", () =>
                {
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
