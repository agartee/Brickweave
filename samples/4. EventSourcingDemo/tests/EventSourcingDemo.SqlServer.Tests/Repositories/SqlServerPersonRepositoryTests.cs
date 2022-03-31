using System;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.People.Models;
using EventSourcingDemo.Domain.Tests.TestHelpers.Builders;
using EventSourcingDemo.SqlServer.Repositories;
using EventSourcingDemo.SqlServer.Tests.Fixtures;
using EventSourcingDemo.SqlServer.Tests.TestHelpers.Seeders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventSourcingDemo.SqlServer.Tests.Repositories
{
    [Trait("Category", "Integration")]
    [Collection("SqlServerTestCollection")]
    public class SqlServerPersonRepositoryTests
    {
        private readonly SqlServerTestFixture _fixture;
        private readonly SqlServerPersonRepository _repository;

        public SqlServerPersonRepositoryTests(SqlServerTestFixture fixture)
        {
            _fixture = fixture;
            _repository = new SqlServerPersonRepository(
                fixture.CreateDbContext());
        }

        [Fact]
        public async Task SavePersonAsync_SavesData()
        {
            await _fixture.ClearDataAsync();

            var person = new PersonBuilder()
                .WithId(PersonId.NewId())
                .WithName(new Name("Person 1"))
                .Build();

            await _repository.SavePersonAsync(person);

            var data = await _fixture.CreateDbContext().People
                .SingleAsync(r => r.Id == person.Id.Value);

            data.Id.Should().Be(person.Id.Value);
            data.Name.Should().Be(person.Name.Value);
        }

        [Fact]
        public async Task DemandPersonAsync_WhenDataExists_ReturnsPerson()
        {
            await _fixture.ClearDataAsync();

            var person = new PersonBuilder()
                .WithId(PersonId.NewId())
                .WithName(new Name("Person 1"))
                .Build();

            var seeder = new PersonSeeder(_fixture.CreateDbContext());
            await seeder.SeedAsync(person);

            var result = await _repository.DemandPersonAsync(person.Id);

            result.Id.Should().Be(person.Id);
            result.Name.Should().Be(person.Name);
        }

        [Fact]
        public async Task DemandPersonAsync_WhenDataDoesNotExist_Throws()
        {
            await _fixture.ClearDataAsync();

            var func = () => _repository.DemandPersonAsync(Domain.People.Models.PersonId.NewId());

            await func.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task DeletePersonAsync_WhenDataExists_DeletesData()
        {
            await _fixture.ClearDataAsync();

            var person = new PersonBuilder()
                .WithId(PersonId.NewId())
                .WithName(new Name("Person 1"))
                .Build();

            var seeder = new PersonSeeder(_fixture.CreateDbContext());
            await seeder.SeedAsync(person);

            await _repository.DeletePersonAsync(person.Id);

            var data = await _fixture.CreateDbContext().People
                .SingleOrDefaultAsync(r => r.Id == person.Id.Value);

            data.Should().BeNull();
        }

        [Fact]
        public async Task DeletePersonAsync_WhenDataDoesNotExist_DoesNotThrow()
        {
            await _fixture.ClearDataAsync();

            var func = () => _repository.DeletePersonAsync(Domain.People.Models.PersonId.NewId());

            await func.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ListPeopleAsync_ReturnsAllPeople()
        {
            await _fixture.ClearDataAsync();

            var person1 = new PersonBuilder()
                .WithId(PersonId.NewId())
                .WithName(new Name("Person 1"))
                .Build();

            var person2 = new PersonBuilder()
                .WithId(PersonId.NewId())
                .WithName(new Name("Person 2"))
                .Build();

            var seeder = new PersonSeeder(_fixture.CreateDbContext());
            await seeder.SeedAsync(person1);
            await seeder.SeedAsync(person2);

            var results = await _repository.ListPeopleAsync();

            results.Should().HaveCount(2);

            var person1Result = results.First(c => c.Id.Equals(person1.Id));
            person1Result.Name.Should().Be(person1.Name);

            var person2Result = results.First(c => c.Id.Equals(person2.Id));
            person2Result.Name.Should().Be(person2.Name);
        }

    }
}
