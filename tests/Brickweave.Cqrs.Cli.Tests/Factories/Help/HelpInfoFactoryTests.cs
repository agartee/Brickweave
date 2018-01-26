using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.Factories.Help;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Readers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Factories.Help
{
    public class HelpInfoFactoryTests
    {
        [Fact]
        public void Create_WhenCategoryAndSubCategoryExist_ReturnsHelpInfoWithChildCategory()
        {
            var categoryHelpReader = Substitute.For<ICategoryHelpReader>();
            categoryHelpReader.GetHelpInfo(Arg.Any<HelpAdjacencyCriteria>()).Returns(
                new HelpInfo("foo", null, "Foo description", HelpInfoType.Category,
                    new List<HelpInfo> { new HelpInfo("bar", "foo", "Bar description", HelpInfoType.Category) }));

            var factory = new HelpInfoFactory(
                categoryHelpReader,
                Substitute.For<IExecutableHelpReader>());

            var result = factory.Create(new[] { "foo", "--help" });

            result.Name.Should().Be("foo");
            result.Subject.Should().BeNullOrWhiteSpace();
            result.Description.Should().Be("Foo description");
            result.Type.Should().Be(HelpInfoType.Category);

            result.Children.Should().HaveCount(1);
            var child = result.Children.First();
            child.Name.Should().Be("bar");
            child.Subject.Should().Be("foo");
            child.Description.Should().Be("Bar description");
            child.Type.Should().Be(HelpInfoType.Category);
        }

        [Fact]
        public void Create_WhenCategoryAndExecutableExists_ReturnsHelpInfoWithChildExecutable()
        {
            var categoryHelpReader = Substitute.For<ICategoryHelpReader>();
            categoryHelpReader.GetHelpInfo(Arg.Any<HelpAdjacencyCriteria>()).Returns(
                new HelpInfo("foo", "foo", "Foo description", HelpInfoType.Category));

            var executableHelpReader = Substitute.For<IExecutableHelpReader>();
            executableHelpReader.GetHelpInfo(Arg.Any<HelpAdjacencyCriteria>()).Returns(new List<HelpInfo>()
            {
                new HelpInfo("create", "foo", "Create Foo description", HelpInfoType.Executable)
            });

            var factory = new HelpInfoFactory(
                categoryHelpReader,
                executableHelpReader);

            var result = factory.Create(new[] { "foo", "--help" });

            result.Name.Should().Be("foo");
            result.Subject.Should().Be("foo");
            result.Description.Should().Be("Foo description");
            result.Type.Should().Be(HelpInfoType.Category);

            result.Children.Should().HaveCount(1);
            var child = result.Children.First();
            child.Name.Should().Be("create");
            child.Subject.Should().Be("foo");
            child.Description.Should().Be("Create Foo description");
            child.Type.Should().Be(HelpInfoType.Executable);
        }

        [Fact]
        public void Create_WhenCategoryDoesNotExistButExecutablesForCategoryExist_ReturnsHelpInfoWithExecutableChildren()
        {
            var executableHelpReader = Substitute.For<IExecutableHelpReader>();
            executableHelpReader.GetHelpInfo(Arg.Any<HelpAdjacencyCriteria>()).Returns(new List<HelpInfo>()
            {
                new HelpInfo("create", "foo", "Create Foo description", HelpInfoType.Executable)
            });

            var factory = new HelpInfoFactory(
                Substitute.For<ICategoryHelpReader>(),
                executableHelpReader);

            var result = factory.Create(new[] { "foo", "--help" });

            result.Name.Should().Be("foo");
            result.Subject.Should().Be("foo");
            result.Description.Should().BeNullOrWhiteSpace();
            result.Type.Should().Be(HelpInfoType.Category);

            result.Children.Should().HaveCount(1);
            var child = result.Children.First();
            child.Name.Should().Be("create");
            child.Subject.Should().Be("foo");
            child.Description.Should().Be("Create Foo description");
            child.Type.Should().Be(HelpInfoType.Executable);
        }
    }
}
