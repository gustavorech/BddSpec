using System;
using BddSpec;

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
                        throw new Exception("failure");
                    });
                });

                When("when_2", () =>
                {
                    throw new Exception("failure");

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
