using System.Linq;
using System.Threading.Tasks;
using Brickweave.Samples.Domain.Persons.Commands;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.Domain.Projections;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace Brickweave.Samples.Projection.Tests
{
    [Trait("Category", "Integration")]
    public class PersonProjectionHandlerTests
    {
        private readonly Person _person;
        private readonly SamplesProjectionDbContext _samplesProjectionDbContext;
        private readonly CreatePersonProjectionHandler _createPersonProjectionHandler; 

        public PersonProjectionHandlerTests()
        {
            _person = new Person(PersonId.NewId(), new Name("Hui", "Li"));
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetPersonAsync(Arg.Any<PersonId>()).Returns(_person);

            var builder = new DbContextOptionsBuilder<SamplesProjectionDbContext>();
            builder.UseInMemoryDatabase("projection_test");

            _samplesProjectionDbContext = new SamplesProjectionDbContext(builder.Options);


            _createPersonProjectionHandler = 
                new CreatePersonProjectionHandler(_samplesProjectionDbContext, personRepository);
        }

        [Fact]
        public async Task CreatePersonCommand_CreatesPersonProjection()
        {
            var createPersonCommand = new CreatePerson(_person.Id, _person.Name);
            await _createPersonProjectionHandler.HandleAsync(createPersonCommand);

            var personProjection = _samplesProjectionDbContext.PersonProjection.First(x => x.Id == _person.Id.Value);

            personProjection.Should().NotBeNull();
            personProjection.Id.Should().Be(_person.Id.Value);
            personProjection.FirstName.Should().Be(_person.Name.FirstName);
            personProjection.LastName.Should().Be(_person.Name.LastName);
        }
    }
}
