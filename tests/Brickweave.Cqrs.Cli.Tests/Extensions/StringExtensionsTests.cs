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
            var input = "\"adam gartee\" was here";

            var result = input.ParseCommandText();

            result[0].Should().Be("adam gartee");
            result[1].Should().Be("was");
            result[2].Should().Be("here");
        }

        [Fact]
        public void ParseCommandText_WithWordsWrappedInDoubleQuotesAndContainsComma_ReturnsCorrectArray()
        {
            var input = "\"gartee, adam\" was here";

            var result = input.ParseCommandText();

            result[0].Should().Be("gartee, adam");
            result[1].Should().Be("was");
            result[2].Should().Be("here");
        }

        [Fact]
        public void ParseCommandText_WithTrailingCommaInValue_ReturnsCorrectArray()
        {
            var input = "create foo --id 12345 --values a,b,c --iscool";

            var result = input.ParseCommandText();

            result[0].Should().Be("create");
            result[1].Should().Be("foo");
            result[2].Should().Be("--id");
            result[3].Should().Be("12345");
            result[4].Should().Be("--values");
            result[5].Should().Be("a~b~c");
            result[6].Should().Be("--iscool");
        }

        [Fact]
        public void ParseCommandText_WithSpacesAndTrailingCommaInValue_ReturnsCorrectArray()
        {
            var input = "create foo --id 12345 --values a, b, c --iscool";

            var result = input.ParseCommandText();

            result[0].Should().Be("create");
            result[1].Should().Be("foo");
            result[2].Should().Be("--id");
            result[3].Should().Be("12345");
            result[4].Should().Be("--values");
            result[5].Should().Be("a~b~c");
            result[6].Should().Be("--iscool");
        }

        [Fact]
        public void ParseCommandText_WithWordsWrappedInDoubleQuotesAndTrailingCommaInValue_ReturnsCorrectArray()
        {
            var input = "create foo --id 12345 --values \"a\", \"b\", \"c\" --iscool";

            var result = input.ParseCommandText();

            result[0].Should().Be("create");
            result[1].Should().Be("foo");
            result[2].Should().Be("--id");
            result[3].Should().Be("12345");
            result[4].Should().Be("--values");
            result[5].Should().Be("a~b~c");
            result[6].Should().Be("--iscool");
        }
    }
}