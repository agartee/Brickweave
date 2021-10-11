namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class GetWaldo : IQuery<WaldoId>
    {
        public WaldoId Id { get; init; }
        public string Name { get; init; }
    }
}
