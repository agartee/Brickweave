using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Companies.Extensions;
using EventSourcingDemo.Domain.Companies.Models;
using EventSourcingDemo.Domain.Companies.Services;

namespace EventSourcingDemo.Domain.Companies.Commands
{
    public class CreateCompanyHandler : ICommandHandler<CreateCompany, CompanyInfo>
    {
        private readonly ICompanyRepository _companyRepository;

        public CreateCompanyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyInfo> HandleAsync(CreateCompany command)
        {
            var company = new Company(CompanyId.NewId(), command.Name);

            await _companyRepository.SaveCompanyAsync(company);

            return company.ToCompanyInfo();
        }
    }
}
