using EventSourcingDemo.Domain.Companies.Models;
using EventSourcingDemo.Domain.Companies.Services;
using EventSourcingDemo.SqlServer.Entities;
using EventSourcingDemo.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventSourcingDemo.SqlServer.Repositories
{
    public class SqlServerCompanyRepository : ICompanyRepository
    {
        private readonly EventSourcingDemoDbContext _dbContext;

        public SqlServerCompanyRepository(EventSourcingDemoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Company> DemandCompanyAsync(CompanyId id)
        {
            var data = await _dbContext.Companies
                .SingleAsync(p => p.Id == id.Value);

            return data.ToCompany();
        }

        public async Task SaveCompanyAsync(Company company)
        {
            var data = await _dbContext.Companies
                .SingleOrDefaultAsync(p => p.Id == company.Id.Value);

            if (data == null)
                data = CreateData(company);
            else
                UpdateData(company, data);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CompanyInfo>> ListCompaniesAsync()
        {
            var data = await _dbContext.Companies
                .ToListAsync();

            return data
                .Select(p => p.ToCompanyInfo())
                .ToList();
        }

        public async Task DeleteCompanyAsync(CompanyId id)
        {
            var data = await _dbContext.Companies
                .SingleOrDefaultAsync(p => p.Id == id.Value);

            if (data == null)
                return;

            _dbContext.Companies.Remove(data);
            await _dbContext.SaveChangesAsync();
        }

        private CompanyData CreateData(Company company)
        {
            var data = new CompanyData
            {
                Id = company.Id.Value,
                Name = company.Name.Value,
            };
            _dbContext.Companies.Add(data);

            return data;
        }

        private static void UpdateData(Company company, CompanyData data)
        {
            data.Name = company.Name.Value;
        }
    }
}
