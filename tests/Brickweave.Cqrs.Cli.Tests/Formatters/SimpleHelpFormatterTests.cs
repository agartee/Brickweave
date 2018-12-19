using Brickweave.Cqrs.Cli.Formatters;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Tests.Builders;
using Xunit;
using Xunit.Abstractions;

namespace Brickweave.Cqrs.Cli.Tests.Formatters
{
    public class SimpleHelpFormatterTests
    {
        private readonly ITestOutputHelper _output;

        public SimpleHelpFormatterTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Format_WithSimpleCategoryHelpInfo_ReturnsFormattedString()
        {
            var helpInfo = new HelpInfoBuilder()
                .WithName("person")
                .WithSubject("person")
                .WithDescription("manage person data")
                .WithType(HelpInfoType.Category)
                .WithChild(new HelpInfoBuilder()
                    .WithName("create")
                    .WithSubject("person")
                    .WithDescription("create new person with a long description so that we can see it wrap to the next line and still be formatted nicely.")
                    .WithType(HelpInfoType.Executable))
                .WithChild(new HelpInfoBuilder()
                    .WithName("delete")
                    .WithSubject("person")
                    .WithDescription("delete an existing person with a long description so that we can see it wrap to the next line and still be formatted nicely.")
                    .WithType(HelpInfoType.Executable))
                .Build();
            
            var result = SimpleHelpFormatter.Format(helpInfo, 80);

            // note: visual assertion: test console output does not match Windows console output exactly
            _output.WriteLine(result);
        }

        [Fact]
        public void Format_WithSimpleCommandHelpInfo_ReturnsFormattedString()
        {
            var helpInfo = new HelpInfoBuilder()
                .WithName("create")
                .WithSubject("person")
                .WithDescription("create new person")
                .WithType(HelpInfoType.Executable)
                .WithChild(new HelpInfoBuilder()
                    .WithName("name")
                    .WithSubject("pilot create")
                    .WithDescription("new person's name")
                    .WithType(HelpInfoType.Parameter))
                .WithChild(new HelpInfoBuilder()
                    .WithName("address")
                    .WithSubject("pilot create")
                    .WithDescription("new person's address")
                    .WithType(HelpInfoType.Parameter))
                .Build();
            
            var result = SimpleHelpFormatter.Format(helpInfo, 80);

            // note: visual assertion: test console output does not match Windows console output exactly
            _output.WriteLine(result);
        }

        [Fact]
        public void Format_WithSimpleCommandHelpInfoIncludingWideTextAndParentheses_ReturnsFormattedString()
        {
            var helpInfo = new HelpInfoBuilder()
                .WithName("create")
                .WithSubject("person")
                .WithDescription("someClass.SomeMethod(\"{D36DFCD7-CD35-40BC-81D6-263C614459A6}\", \"{D36DFCD7-CD35-40BC-81D6-263C614459A6}\")")
                .WithType(HelpInfoType.Executable)
                .Build();

            var result = SimpleHelpFormatter.Format(helpInfo, 80);

            // note: visual assertion: test console output does not match Windows console output exactly
            _output.WriteLine(result);
        }
    }
}