namespace Brickweave.Cqrs.Tests.Models
{
    public class Result
    {
        public Result(string value)
        {
            Value = value;
        }

        public string Value { get; }

        private bool Equals(Result other)
        {
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Result)obj);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }
}