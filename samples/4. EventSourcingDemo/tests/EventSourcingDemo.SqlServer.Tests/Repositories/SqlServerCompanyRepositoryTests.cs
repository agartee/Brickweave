using System;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.Companies.Models;
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
    public class SqlServerCompanyRepositoryTests
    {
        private readonly SqlServerTestFixture _fixture;
        private readonly SqlServerCompanyRepository _repository;

        public SqlServerCompanyRepositoryTests(SqlServerTestFixture fixture)
        {
            _fixture = fixture;
            _repository = new SqlServerCompanyRepository(
                fixture.CreateDbContext());
        }

        [Fact]
        public async Task SaveCompanyAsync_SavesData()
        {
            await _fixture.ClearDataAsync();

            var company = new CompanyBuilder()
                .WithId(CompanyId.NewId())
                .WithName(new Name("Company 1"))
                .Build();

            await _repository.SaveCompanyAsync(company);

            var data = await _fixture.CreateDbContext().Companies
                .SingleAsync(r => r.Id == company.Id.Value);

            data.Id.Should().Be(company.Id.Value);
            data.Name.Should().Be(company.Name.Value);
        }

        [Fact]
        public async Task DemandCompanyAsync_WhenDataExists_ReturnsCompany()
        {
            await _fixture.ClearDataAsync();

            var company = new CompanyBuilder()
                .WithId(CompanyId.NewId())
                .WithName(new Name("Company 1"))
                .Build();

            var seeder = new CompanySeeder(_fixture.CreateDbContext());
            await seeder.SeedAsync(company);

            var result = await _repository.DemandCompanyAsync(company.Id);

            result.Id.Should().Be(company.Id);
            result.Name.Should().Be(company.Name);
        }

        [Fact]
        public async Task DemandCompanyAsync_WhenDataDoesNotExist_Throws()
        {
            await _fixture.ClearDataAsync();

            var func = () => _repository.DemandCompanyAsync(CompanyId.NewId());

            await func.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task DeleteCompanyAsync_WhenDataExists_DeletesData()
        {
            await _fixture.ClearDataAsync();

            var company = new CompanyBuilder()
                .WithId(CompanyId.NewId())
                .WithName(new Name("Company 1"))
                .Build();

            var seeder = new CompanySeeder(_fixture.CreateDbContext());
            await seeder.SeedAsync(company);

            await _repository.DeleteCompanyAsync(company.Id);

            var data = await _fixture.CreateDbContext().Companies
                .SingleOrDefaultAsync(r => r.Id == company.Id.Value);

            data.Should().BeNull();
        }

        [Fact]
        public async Task DeleteCompanyAsync_WhenDataDoesNotExist_DoesNotThrow()
        {
            await _fixture.ClearDataAsync();

            var func = () => _repository.DeleteCompanyAsync(CompanyId.NewId());

            await func.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ListCompaniesAsync_ReturnsAllCompanies()
        {
            await _fixture.ClearDataAsync();

            var company1 = new CompanyBuilder()
                .WithId(CompanyId.NewId())
                .WithName(new Name("Company 1"))
                .Build();

            var company2 = new CompanyBuilder()
                .WithId(CompanyId.NewId())
                .WithName(new Name("Company 2"))
                .Build();

            var seeder = new CompanySeeder(_fixture.CreateDbContext());
            await seeder.SeedAsync(company1);
            await seeder.SeedAsync(company2);

            var results = await _repository.ListCompaniesAsync();

            results.Should().HaveCount(2);
            
            var company1Result = results.First(c => c.Id.Equals(company1.Id));
            company1Result.Name.Should().Be(company1.Name);

            var company2Result = results.First(c => c.Id.Equals(company2.Id));
            company2Result.Name.Should().Be(company2.Name);
        }
    }
}
