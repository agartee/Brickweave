using System;
using System.Linq;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Readers;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Readers
{
    public class JsonFileCategoryHelpReaderTests
    {
        [Fact]
        public void GetHelpInfo_WhenFileContainsData_ReturnsHelpInfoForCategoryAndOneLevelDeep()
        {
            var reader = new JsonFileCategoryHelpReader("Data\\Categories.json");

            var result = reader.GetHelpInfo(new HelpAdjacencyCriteria("category1"));
            
            result.Name.Should().Be("category1");
            result.Subject.Should().Be("category1");
            result.Description.Should().Be("category1 description");
            result.Type.Should().Be(HelpInfoType.Category);

            result.Children.Should().HaveCount(1);
            var child = result.Children.First();
            child.Name.Should().Be("subcategory1");
            child.Subject.Should().Be("category1");
            child.Description.Should().Be("subcategory1 description");
            child.Type.Should().Be(HelpInfoType.Category);
        }

        [Fact]
        public void GetHelpInfo_WhenFileContainsDataAndPassingSubCategory_ReturnsHelpInfoForCategoryAndOneLevelDeep()
        {
            var reader = new JsonFileCategoryHelpReader("Data\\Categories.json");

            var result = reader.GetHelpInfo(new HelpAdjacencyCriteria("category1 subcategory1"));

            result.Name.Should().Be("subcategory1");
            result.Subject.Should().Be("category1");
            result.Description.Should().Be("subcategory1 description");
            result.Type.Should().Be(HelpInfoType.Category);

            result.Children.Should().HaveCount(1);
            var child = result.Children.First();
            child.Name.Should().Be("subcategory2");
            child.Subject.Should().Be("subcategory1");
            child.Description.Should().Be("subcategory2 description");
            child.Type.Should().Be(HelpInfoType.Category);
        }

        [Fact]
        public void GetHelpInfo_WhenEmptyAdjacencyCriteria_ReturnsDefaultInfoWithFirstTierCategories()
        {
            var reader = new JsonFileCategoryHelpReader("Data\\Categories.json",
                new HelpInfo("default", "subject", "description", HelpInfoType.Category));

            var result = reader.GetHelpInfo(HelpAdjacencyCriteria.Empty());

            result.Name.Should().Be("default");
            result.Subject.Should().Be("subject");
            result.Description.Should().Be("description");
            result.Type.Should().Be(HelpInfoType.Category);

            result.Children.Should().HaveCount(2);
            var child1 = result.Children.First(h => h.Name == "category1");
            child1.Name.Should().Be("category1");
            child1.Subject.Should().Be("category1");
            child1.Description.Should().Be("category1 description");
            child1.Type.Should().Be(HelpInfoType.Category);

            var child2 = result.Children.First(h => h.Name == "category2");
            child2.Name.Should().Be("category2");
            child2.Subject.Should().Be("category2");
            child2.Description.Should().Be("category2 description");
            child2.Type.Should().Be(HelpInfoType.Category);
        }

        [Fact]
        public void GetHelpInfo_WhenFileContainsNoData_ReturnsDefaultHelpInfo()
        {
            var reader = new JsonFileCategoryHelpReader("Data\\Categories-Empty.json",
                new HelpInfo("default", "subject", "description", HelpInfoType.Category));
            var result = reader.GetHelpInfo(HelpAdjacencyCriteria.Empty());

            result.Name.Should().Be("default");
            result.Subject.Should().Be("subject");
            result.Description.Should().Be("description");
            result.Type.Should().Be(HelpInfoType.Category);
        }

        [Fact]
        public void GetHelpInfo_WhenFileDoesNotContainQueriedData_ReturnsNull()
        {
            var reader = new JsonFileCategoryHelpReader("Data\\Categories.json",
                new HelpInfo("default", "subject", "description", HelpInfoType.Category));
            var result = reader.GetHelpInfo(new HelpAdjacencyCriteria("non-existent"));

            result.Should().BeNull();
        }

        [Fact]
        public void GetHelpInfo_WhenHelpAdjacencyIsNull_Throws()
        {
            var reader = new JsonFileCategoryHelpReader("Data\\Categories-IncorrectlyFormatted.txt");

            Assert.Throws<ArgumentNullException>(() => reader.GetHelpInfo(null));
        }

        [Fact]
        public void GetHelpInfo_WhenFileIsNotCorrectlyFormatted_Throws()
        {
            var reader = new JsonFileCategoryHelpReader("Data\\Categories-IncorrectlyFormatted.txt");

            Assert.Throws<CategoryHelpFileInvalidExeption>(() => reader.GetHelpInfo(HelpAdjacencyCriteria.Empty()));
        }

        [Fact]
        public void GetHelpInfo_WhenFileMissing_Throws()
        {
            var reader = new JsonFileCategoryHelpReader("Data\\Categories-DoesNotExist.json");

            Assert.Throws<CategoryHelpFileNotFoundExeption>(() => reader.GetHelpInfo(HelpAdjacencyCriteria.Empty()));
        }
    }
}
