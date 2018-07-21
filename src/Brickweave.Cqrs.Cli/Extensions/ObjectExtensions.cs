namespace Brickweave.Cqrs.Cli.Extensions
{
    public static class ObjectExtensions
    {
        public static dynamic AsDynamic(this object obj)
        {
            return (dynamic) obj;
        }
    }
}
