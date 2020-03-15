using System.Linq;
using System;
using Xunit;
using System.IO;
using BddSpec;
using FluentAssertions;

namespace IntegrationTests
{
    [Collection("Not run in parallel")]
    public class VerbosityConfigTest
    {
        static string REDUCED_EXECUTION = @"
> Executing specs
.FF.
";

        static string VERBOSE_EXECUTION = @"
> Executing specs
IntegrationTests.WithFailureSpec
  context context_1
    when when_1
      it it_1
      it it_2
    when when_2
    when when_3
";

        static string SUMMARY_EXECUTION = @"
> Showing summary of the execution
IntegrationTests.WithFailureSpec
  context context_1
    when when_1
      it it_1
      it it_2
    when when_2
    when when_3
";

        static string ERROR_SUMMARY_PRINT = @"
> Showing summary of failures (see details above)
IntegrationTests.WithFailureSpec
  context context_1:10
    when when_1:12
      it it_2:19
    when when_2:25

Spec classes with failures:
  - IntegrationTests.WithFailureSpec
";

        [Theory]
        [InlineData("WithFailureSpec")]
        [InlineData("WithFailureSpec", "-v", "e")]
        [InlineData("WithFailureSpec", "-v", "errors")]
        public void PrintOnlyErrors(params string[] args)
        {
            var output = new StringWriter();
            Console.SetOut(output);

            new BddSpecManager(false).Execute(args);

            string result = output.ToString();

            result.Should().Contain(REDUCED_EXECUTION);
            result.Should().NotContain(VERBOSE_EXECUTION);
            result.Should().NotContain(SUMMARY_EXECUTION);
            result.Should().Contain(ERROR_SUMMARY_PRINT);
        }

        [Theory]
        [InlineData("WithFailureSpec", "-v", "v")]
        [InlineData("WithFailureSpec", "-v", "verbose")]
        public void PrintVerbose(params string[] args)
        {
            var output = new StringWriter();
            Console.SetOut(output);

            new BddSpecManager(false).Execute(args);

            string result = output.ToString();

            result.Should().NotContain(REDUCED_EXECUTION);
            result.Should().Contain(VERBOSE_EXECUTION);
            result.Should().NotContain(SUMMARY_EXECUTION);
            result.Should().Contain(ERROR_SUMMARY_PRINT);
        }

        [Theory]
        [InlineData("WithFailureSpec", "-v", "s")]
        [InlineData("WithFailureSpec", "-v", "summary")]
        public void PrintSummary(params string[] args)
        {
            var output = new StringWriter();
            Console.SetOut(output);

            new BddSpecManager(false).Execute(args);

            string result = output.ToString();

            result.Should().Contain(REDUCED_EXECUTION);
            result.Should().NotContain(VERBOSE_EXECUTION);
            result.Should().Contain(SUMMARY_EXECUTION);
            result.Should().Contain(ERROR_SUMMARY_PRINT);
        }
    }
}
