using Brickweave.Cqrs.Cli.Extensions;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void UpperCaseFirst_WhenAllLowerCase_ReturnsInputWithUpperCaseFirstLetter()
        {
            var input = "adam";

            var result = input.UppercaseFirst();

            result.Should().Be("Adam");
        }

        [Fact]
        public void ParseCommandText_WithSimpleWords_ReturnsCorrectArray()
        {
            var input = "adam was here";

            var result = input.ParseCommandText();

            result[0].Should().Be("adam");
            result[1].Should().Be("was");
            result[2].Should().Be("here");
        }

        [Fact]
        public void ParseCommandText_WithWordsWrappedInDoubleQuotes_ReturnsCorrectArray()
        {
            var input = @"""adam gartee"" was here";

            var result = input.ParseCommandText();

            result[0].Should().Be("adam gartee");
            result[1].Should().Be("was");
            result[2].Should().Be("here");
        }

        [Fact]
        public void ParseCommandText_WithWordsWrappedInMiddleOfText_ReturnsCorrectArray()
        {
            var input = @"an ""adam gartee"" was here";

            var result = input.ParseCommandText();

            result[0].Should().Be("an");
            result[1].Should().Be("adam gartee");
            result[2].Should().Be("was");
            result[3].Should().Be("here");
        }

        [Fact]
        public void ParseCommandText_WhenDoubleQuotesAreRequiredInOutput_ReturnsCorrectArray()
        {
            var input = "\"adam gartee\" was here";

            var result = input.ParseCommandText();

            result[0].Should().Be("adam gartee");
            result[1].Should().Be("was");
            result[2].Should().Be("here");
        }

        [Fact]
        public void Wrap_WithStringNotExceedingLimit_ReturnsSingleLine()
        {
            var input = "Adam was here";

            var result = input.Wrap(15);

            result[0].Should().Be(input);
        }

        [Fact]
        public void Wrap_WithStringExceedingLimit_ReturnsTwoLines()
        {
            var input = "Adam was here";

            var result = input.Wrap(10);

            result[0].Should().Be("Adam was");
            result[1].Should().Be("here");
        }
    }
}