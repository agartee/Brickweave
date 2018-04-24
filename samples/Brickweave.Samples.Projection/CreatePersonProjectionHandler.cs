using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Commands;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.Projection.Entities;

namespace Brickweave.Samples.Projection
{
    public class CreatePersonProjectionHandler : IProjectionHandler<CreatePerson>
    {
        private readonly SamplesProjectionDbContext _sampleProjectionDbContext;
        private readonly IPersonRepository _personalRepository;

        public CreatePersonProjectionHandler(SamplesProjectionDbContext sampleProjectionDbContext, IPersonRepository personRepository)
        {
            _sampleProjectionDbContext = sampleProjectionDbContext;
            _personalRepository = personRepository;
        }

        public async Task HandleAsync(CreatePerson command)
        {
            var person = await _personalRepository.GetPersonAsync(command.Id);
            var personInfo = person.ToInfo();

            var personProjection = new PersonProjection(
                personInfo.Id.Value, personInfo.Name.FirstName, personInfo.Name.LastName);

            await _sampleProjectionDbContext.AddAsync(personProjection);
            await _sampleProjectionDbContext.SaveChangesAsync();
        }
    }
}
