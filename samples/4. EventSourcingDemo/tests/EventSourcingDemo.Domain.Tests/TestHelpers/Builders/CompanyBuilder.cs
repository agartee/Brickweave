using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.Companies.Models;

namespace EventSourcingDemo.Domain.Tests.TestHelpers.Builders
{
    public class CompanyBuilder
    {
        private CompanyId _id = CompanyId.NewId();
        private Name _name;

        public CompanyBuilder WithId(CompanyId id)
        {
            _id = id;
            return this;
        }

        public CompanyBuilder WithName(Name name)
        {
            _name = name;
            return this;
        }

        public Company Build()
        {
            return new Company(
                _id, 
                _name ?? new Name(_id.ToString()));
        }
    }
}
