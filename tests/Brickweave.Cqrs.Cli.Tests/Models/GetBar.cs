namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class GetBar : IQuery<BarId>
    {
        public GetBar(BarId id, string name)
        {
            Id = id;
            Name = name;
        }

        public BarId Id { get; }
        public string Name { get; }
    }
}
