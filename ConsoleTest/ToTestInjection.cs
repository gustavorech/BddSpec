﻿using System;
using System.Threading;
using Autofac.Extras.Moq;
using BddSpec;
using FluentAssertions;
using Moq;

namespace ConsoleTest
{
    public interface IInject
    {
        string M1();
        string M2();
    }

    public class InjectedClass
    {
        public IInject Inject { get; }

        public InjectedClass(IInject inject)
        {
            Inject = inject;
        }

        public string Call()
        {
            string value = Inject.M1();
            if (!string.IsNullOrEmpty(value))
                return value;

            return Inject.M2();
        }
    }

    public class ToTestInjection : SpecClass
    {
        private AutoMock Auto = AutoMock.GetLoose();

        public override void SetupSpecs()
        {
            var testClass = Auto.Create<InjectedClass>();
            string m1_return = "m1 return";
            string m2_return = "m2 return";

            Method("Call", () =>
            {
                When("M1 returns a value", () =>
                {
                    Auto.Mock<IInject>()
                        .Setup(c => c.M1())
                        .Returns(m1_return);

                    var result = testClass.Call();

                    It("returns the M1 value", () => result.Should().Be(m1_return));

                    It("do not call M2", () =>
                    {
                        Auto.Mock<IInject>().Verify(c => c.M2(), Times.Never);
                    });
                });

                When("M2 returns a value", () =>
                {
                    Auto.Mock<IInject>()
                        .Setup(c => c.M2())
                        .Returns(m2_return);

                    var result = testClass.Call();

                    It("returns the M2 value", () => result.Should().Be(m1_return));
                });

                When("M1 and M2 dos not return value", () =>
                {
                    var result = testClass.Call();

                    It("should return null", () => result.Should().BeNull());
                });
            });

            Method("IAmICaller", () =>
            {
                When("M1 returns a value", () =>
                {
                    Auto.Mock<IInject>()
                        .Setup(c => c.M1())
                        .Returns(m1_return);

                    var result = testClass.Call();

                    It("returns the M1 value", () => result.Should().Be(m1_return));

                    It("do not call M2", () =>
                    {
                        Auto.Mock<IInject>().Verify(c => c.M2(), Times.Never);
                    });
                });

                When("M2 returns a value", () =>
                {
                    Auto.Mock<IInject>()
                        .Setup(c => c.M2())
                        .Returns(m2_return);

                    var result = testClass.Call();

                    It("returns the M2 value", () => result.Should().Be(m1_return));
                });

                When("M1 and M2 dos not return value", () =>
                {
                    var result = testClass.Call();

                    It("should return null", () => result.Should().BeNull());
                });
            });


        }

        ~ToTestInjection()
        {
            Auto.Dispose();
        }
    }

    public class ToTestInjection2 : SpecClass
    {
        private AutoMock Auto = AutoMock.GetLoose();

        public override void SetupSpecs()
        {
            var testClass = Auto.Create<InjectedClass>();
            string m1_return = "m1 return";
            string m2_return = "m2 return";

            Method("Call", () =>
            {
                When("M1 returns a value", () =>
                {
                    Auto.Mock<IInject>()
                        .Setup(c => c.M1())
                        .Returns(m1_return);

                    var result = testClass.Call();

                    It("returns the M1 value", () => result.Should().Be(m1_return));

                    It("do not call M2", () =>
                    {
                        Auto.Mock<IInject>().Verify(c => c.M2(), Times.Never);
                    });
                });

                When("M2 returns a value", () =>
                {
                    Auto.Mock<IInject>()
                        .Setup(c => c.M2())
                        .Returns(m2_return);

                    var result = testClass.Call();

                    It("returns the M2 value", () => result.Should().Be(m1_return));
                });

                When("M1 and M2 dos not return value", () =>
                {
                    var result = testClass.Call();

                    It("should return null", () => result.Should().BeNull());
                });
            });

            Method("IAmICaller", () =>
            {
                When("M1 returns a value", () =>
                {
                    Auto.Mock<IInject>()
                        .Setup(c => c.M1())
                        .Returns(m1_return);

                    var result = testClass.Call();

                    It("returns the M1 value", () => result.Should().Be(m1_return));

                    It("do not call M2", () =>
                    {
                        Auto.Mock<IInject>().Verify(c => c.M2(), Times.Never);
                    });
                });

                When("M2 returns a value", () =>
                {
                    Auto.Mock<IInject>()
                        .Setup(c => c.M2())
                        .Returns(m2_return);

                    var result = testClass.Call();

                    It("returns the M2 value", () => result.Should().Be(m1_return));
                });

                When("M1 and M2 dos not return value", () =>
                {
                    var result = testClass.Call();

                    It("should return null", () => result.Should().BeNull());
                });
            });


        }

        ~ToTestInjection2()
        {
            Auto.Dispose();
        }
    }

    public class ToTestInjection3 : SpecClass
    {
        private AutoMock Auto = AutoMock.GetLoose();

        public override void SetupSpecs()
        {
            var testClass = Auto.Create<InjectedClass>();
            string m1_return = "m1 return";
            string m2_return = "m2 return";

            Method("Call", () =>
            {
                When("M1 returns a value", () =>
                {
                    Auto.Mock<IInject>()
                        .Setup(c => c.M1())
                        .Returns(m1_return);

                    var result = testClass.Call();

                    It("returns the M1 value", () => result.Should().Be(m1_return));

                    It("do not call M2", () =>
                    {
                        Auto.Mock<IInject>().Verify(c => c.M2(), Times.Never);
                    });
                });

                When("M2 returns a value", () =>
                {
                    Auto.Mock<IInject>()
                        .Setup(c => c.M2())
                        .Returns(m2_return);

                    var result = testClass.Call();

                    It("returns the M2 value", () => result.Should().Be(m1_return));
                });

                When("M1 and M2 dos not return value", () =>
                {
                    var result = testClass.Call();

                    It("should return null", () => result.Should().BeNull());
                });
            });

            Method("IAmICaller", () =>
            {
                When("M1 returns a value", () =>
                {
                    Auto.Mock<IInject>()
                        .Setup(c => c.M1())
                        .Returns(m1_return);

                    var result = testClass.Call();

                    It("returns the M1 value", () => result.Should().Be(m1_return));

                    It("do not call M2", () =>
                    {
                        Auto.Mock<IInject>().Verify(c => c.M2(), Times.Never);
                    });
                });

                When("M2 returns a value", () =>
                {
                    Auto.Mock<IInject>()
                        .Setup(c => c.M2())
                        .Returns(m2_return);

                    var result = testClass.Call();

                    It("returns the M2 value", () => result.Should().Be(m1_return));
                });

                When("M1 and M2 dos not return value", () =>
                {
                    var result = testClass.Call();

                    It("should return null", () => result.Should().BeNull());
                });
            });


        }

        ~ToTestInjection3()
        {
            Auto.Dispose();
        }
    }
}
