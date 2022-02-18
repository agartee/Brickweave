using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.Companies.Models;

namespace EventSourcingDemo.Domain.Companies.Commands
{
    public class CreateCompany : ICommand<CompanyInfo>
    {
        public CreateCompany(Name name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Name Name { get; }
    }
}
