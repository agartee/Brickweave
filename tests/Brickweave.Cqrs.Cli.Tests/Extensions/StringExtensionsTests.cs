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
        public void ParseCommandText_WithSingleWords_ReturnsCorrectArray()
        {
            var input = "foo list";

            var result = input.ParseCommandText();

            result[0].Should().Be("foo");
            result[1].Should().Be("list");
        }

        [Fact]
        public void ParseCommandText_WithWordsWrappedInDoubleQuotes_ReturnsCorrectArray()
        {
            var input = "foo list --name \"foo's name\"";

            var result = input.ParseCommandText();

            result[0].Should().Be("foo");
            result[1].Should().Be("list");
            result[2].Should().Be("--name");
            result[3].Should().Be("foo's name");
        }

        [Fact]
        public void ParseCommandText_WithWordsWrappedInDoubleQuotesThatContainEscapedDoubleQuotes_ReturnsCorrectArray()
        {
            var input = "foo create --description \"this is a \\\"description\\\" with some escaped quotes\"";

            var result = input.ParseCommandText();

            result[0].Should().Be("foo");
            result[1].Should().Be("create");
            result[2].Should().Be("--description");
            result[3].Should().Be("this is a \\\"description\\\" with some escaped quotes");
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