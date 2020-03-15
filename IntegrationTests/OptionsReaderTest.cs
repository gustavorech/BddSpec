using System.Linq;
using System;
using Xunit;
using System.IO;
using BddSpec;
using FluentAssertions;

namespace IntegrationTests
{
    [Collection("Not run in parallel")]
    public class OptionsReaderTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(235)]
        [InlineData(0)]
        [InlineData(-10)]
        public void HasSpecificLine(int line)
        {
            var output = new StringWriter();
            Console.SetOut(output);

            new BddSpecManager(false).Execute("NoSpec:" + line.ToString());

            Configuration.SpecificLine.Should().Be(line);
            Configuration.IsSpecificLine.Should().BeTrue();

            new BddSpecManager(false).Execute(":" + line.ToString());

            Configuration.SpecificLine.Should().Be(line);
            Configuration.IsSpecificLine.Should().BeTrue();
        }

        [Theory]
        [InlineData("NoSpec")]
        [InlineData("NoSpec:")]
        [InlineData("NoSpec:text")]
        public void HasNotSpecificLine(params string[] args)
        {
            var output = new StringWriter();
            Console.SetOut(output);

            new BddSpecManager(false).Execute(args);

            Configuration.SpecificLine.Should().BeNull();
            Configuration.IsSpecificLine.Should().BeFalse();
        }
    }
}
