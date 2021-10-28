namespace Brickweave.Cqrs.Models
{
    public class ExceptionInfo
    {
        public ExceptionInfo(string name, string message)
        {
            Name = name;
            Message = message;
        }

        public string Name { get; }
        public string Message { get; }

        public override string ToString()
        {
            return $"{Name}: {Message}";
        }
    }
}
