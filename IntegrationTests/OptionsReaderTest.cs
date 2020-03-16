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

        [Theory]
        [InlineData("NoSpec", "-l")]
        [InlineData("NoSpec", "--show-line")]
        public void ShowLine(params string[] args)
        {
            var output = new StringWriter();
            Console.SetOut(output);

            new BddSpecManager(false).Execute(args);

            Configuration.ShowLine.Should().BeTrue();
        }

        [Theory]
        [InlineData("NoSpec")]
        public void DoNotShowLine(params string[] args)
        {
            var output = new StringWriter();
            Console.SetOut(output);

            new BddSpecManager(false).Execute(args);

            Configuration.ShowLine.Should().BeFalse();
        }

        [Theory]
        [InlineData("NoSpec", "-t")]
        [InlineData("NoSpec", "--show-time")]
        public void ShowTime(params string[] args)
        {
            var output = new StringWriter();
            Console.SetOut(output);

            new BddSpecManager(false).Execute(args);

            Configuration.ShowTime.Should().BeTrue();
        }

        [Theory]
        [InlineData("NoSpec")]
        public void DoNotShowTime(params string[] args)
        {
            var output = new StringWriter();
            Console.SetOut(output);

            new BddSpecManager(false).Execute(args);

            Configuration.ShowTime.Should().BeFalse();
        }

        [Theory]
        [InlineData("NoSpec")]
        [InlineData("NonExistentSpec")]
        [InlineData("SomeNamespace.%")]
        [InlineData("%SomeFilter%")]
        public void SpecFilter(params string[] args)
        {
            var output = new StringWriter();
            Console.SetOut(output);

            new BddSpecManager(false).Execute(args);

            Configuration.SpecFilter.Should().Be(args.First());
            Configuration.IsSpecFiltered.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("-l")]
        public void NoSpecFilter(params string[] args)
        {
            var output = new StringWriter();
            Console.SetOut(output);

            new BddSpecManager(false).Execute(args);

            Configuration.SpecFilter.Should().BeNullOrEmpty();
            Configuration.IsSpecFiltered.Should().BeFalse();
        }

        [Theory]
        [InlineData("", 10)]
        [InlineData("NoSpec", 25)]
        [InlineData("%SomeFilter%", 50)]
        public void SeparateSpecFilterAndLine(string specFilter, int line)
        {
            var output = new StringWriter();
            Console.SetOut(output);

            string specFilterWithLine = $"{specFilter}:{line}";
            string[] args = new string[] { specFilterWithLine };

            new BddSpecManager(false).Execute(args);

            Configuration.SpecFilter.Should().Be(specFilter);
            Configuration.SpecificLine.Should().Be(line);
        }
    }
}
