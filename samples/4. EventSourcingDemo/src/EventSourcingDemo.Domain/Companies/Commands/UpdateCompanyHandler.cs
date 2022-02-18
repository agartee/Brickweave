using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Companies.Extensions;
using EventSourcingDemo.Domain.Companies.Models;
using EventSourcingDemo.Domain.Companies.Services;

namespace EventSourcingDemo.Domain.Companies.Commands
{
    public class UpdateCompanyHandler : ICommandHandler<UpdateCompany, CompanyInfo>
    {
        private readonly ICompanyRepository _companyRepository;

        public UpdateCompanyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyInfo> HandleAsync(UpdateCompany command)
        {
            var company = await _companyRepository.DemandCompanyAsync(command.CompanyId);

            company.Name = command.Name;

            await _companyRepository.SaveCompanyAsync(company);

            return company.ToCompanyInfo();
        }
    }
}
