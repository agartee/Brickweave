using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Readers;
using Brickweave.Cqrs.Cli.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Readers
{
    public class XmlDocumentationFileConstructorHelpReaderTests
    {
        [Fact]
        public void GetHelpInfo_WithConstructorHelpDocumentationStrategy_WhenFileContainsDataForSubject_ReturnsHelpInfo()
        {
            var reader = new XmlDocumentationFileConstructorHelpReader(
                "Data\\Executables-ConstructorSummaries.xml");

            var results = reader.GetHelpInfo(new HelpAdjacencyCriteria("foo"))
                .ToArray();

            results.Should().HaveCount(2);

            var result1 = results.First(h => h.Name == "create");
            result1.Name.Should().Be("create");
            result1.Subject.Should().Be("foo");
            result1.Description.Should().Be("Create new Foo");
            result1.Type.Should().Be(HelpInfoType.Executable);
            result1.Children.Should().HaveCount(2);

            var result1Param1 = result1.Children.First(p => p.Name == "id");
            result1Param1.Name.Should().Be("id");
            result1Param1.Subject.Should().Be("foo create");
            result1Param1.Description.Should().Be("Id parameter");
            result1Param1.Type.Should().Be(HelpInfoType.Parameter);
            result1Param1.Children.Should().HaveCount(0);

            var result1Param2 = result1.Children.First(p => p.Name == "bar");
            result1Param2.Name.Should().Be("bar");
            result1Param2.Subject.Should().Be("foo create");
            result1Param2.Description.Should().Be("Bar parameter");
            result1Param2.Type.Should().Be(HelpInfoType.Parameter);
            result1Param2.Children.Should().HaveCount(0);

            var result2 = results.First(h => h.Name == "delete");
            result2.Name.Should().Be("delete");
            result2.Subject.Should().Be("foo");
            result2.Description.Should().Be("Delete existing Foo");
            result2.Type.Should().Be(HelpInfoType.Executable);
            result2.Children.Should().HaveCount(1);

            var result2Param1 = result2.Children.First(p => p.Name == "id");
            result2Param1.Name.Should().Be("id");
            result2Param1.Subject.Should().Be("foo delete");
            result2Param1.Description.Should().Be("Id parameter");
            result2Param1.Type.Should().Be(HelpInfoType.Parameter);
            result2Param1.Children.Should().HaveCount(0);
        }

        [Fact]
        public void GetHelpInfo_WhenFileContainsDataForCommand_ReturnsHelpInfo()
        {
            var reader = new XmlDocumentationFileConstructorHelpReader(
                "Data\\Executables-ConstructorSummaries.xml");

            var results = reader.GetHelpInfo(new HelpAdjacencyCriteria("foo", "create"))
                .ToArray();

            results.Should().HaveCount(1);

            var result1 = results.First(h => h.Name == "create");
            result1.Name.Should().Be("create");
            result1.Subject.Should().Be("foo");
            result1.Description.Should().Be("Create new Foo");
            result1.Type.Should().Be(HelpInfoType.Executable);
            result1.Children.Should().HaveCount(2);

            var result1Param1 = result1.Children.First(p => p.Name == "id");
            result1Param1.Name.Should().Be("id");
            result1Param1.Subject.Should().Be("foo create");
            result1Param1.Description.Should().Be("Id parameter");
            result1Param1.Type.Should().Be(HelpInfoType.Parameter);
            result1Param1.Children.Should().HaveCount(0);

            var result1Param2 = result1.Children.First(p => p.Name == "bar");
            result1Param2.Name.Should().Be("bar");
            result1Param2.Subject.Should().Be("foo create");
            result1Param2.Description.Should().Be("Bar parameter");
            result1Param2.Type.Should().Be(HelpInfoType.Parameter);
            result1Param2.Children.Should().HaveCount(0);
        }

        [Fact]
        public void GetHelpInfo_WithRegisteredCommand_ReturnsHelpInfo()
        {
            var reader = new XmlDocumentationFileConstructorHelpReader(new List<IExecutableRegistration>
                {
                    new ExecutableRegistration<ListFoos>("list", "foo")
                },
                Enumerable.Empty<Type>(),
                "Data\\Executables-ConstructorSummaries.xml");

            var results = reader.GetHelpInfo(new HelpAdjacencyCriteria("foo", "list"))
                .ToArray();

            results.Should().HaveCount(1);

            var result1 = results.First(h => h.Name == "list");
            result1.Name.Should().Be("list");
            result1.Subject.Should().Be("foo");
            result1.Description.Should().Be("List Foos");
            result1.Type.Should().Be(HelpInfoType.Executable);
            result1.Children.Should().HaveCount(0);
        }
        
        [Fact]
        public void GetHelpInfo_WhenFileContainsNoData_ReturnsEmptyList()
        {
            var reader = new XmlDocumentationFileConstructorHelpReader(
                "Data\\Executables-Empty.xml");
            var results = reader.GetHelpInfo(HelpAdjacencyCriteria.Empty());

            results.Should().HaveCount(0);
        }

        [Fact]
        public void GetHelpInfo_WhenHelpAdjacencyIsNull_Throws()
        {
            var reader = new XmlDocumentationFileConstructorHelpReader(
                "Data\\Executables-Empty.xml");

            Assert.Throws<ArgumentNullException>(() =>
                reader.GetHelpInfo(null));
        }

        [Fact]
        public void GetHelpInfo_WhenFileIsNotCorrectlyFormatted_Throws()
        {
            var reader = new XmlDocumentationFileConstructorHelpReader(
                "Data\\Executables-IncorrectlyFormatted.txt");

            Assert.Throws<ExecutableHelpFileInvalidExeption>(() => 
            reader.GetHelpInfo(HelpAdjacencyCriteria.Empty()));
        }

        [Fact]
        public void GetHelpInfo_WhenFileMissing_Throws()
        {
            var reader = new XmlDocumentationFileConstructorHelpReader(
                "DoesNotExist.xml");

            Assert.Throws<ExecutableHelpFileNotFoundExeption>(() => 
            reader.GetHelpInfo(HelpAdjacencyCriteria.Empty()));
        }

        [Fact]
        public void GetHelpInfo_WhenContainsExcludedCommand_ReturnsHelpWithoutExcludedCommand()
        {
            var reader = new XmlDocumentationFileConstructorHelpReader(
                Enumerable.Empty<IExecutableRegistration>(),
                new List<Type>
                {
                    typeof(DeleteBaz)
                },
                "Data\\Executables-ConstructorSummaries.xml");

            var results = reader.GetHelpInfo(new HelpAdjacencyCriteria("baz"))
                .ToArray();

            results.Should().HaveCount(0);
        }
    }
}
