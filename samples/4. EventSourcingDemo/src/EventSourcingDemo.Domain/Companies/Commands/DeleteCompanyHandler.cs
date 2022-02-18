using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Companies.Services;

namespace EventSourcingDemo.Domain.Companies.Commands
{
    public class DeleteCompanyHandler : ICommandHandler<DeleteCompany>
    {
        private readonly ICompanyRepository _companyRepository;

        public DeleteCompanyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task HandleAsync(DeleteCompany command)
        {
            await _companyRepository.DeleteCompanyAsync(command.CompanyId);
        }
    }
}
