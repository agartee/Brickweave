namespace EventSourcing.Domain.Common.Models
{
    public class Name
    {
        public Name(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override bool Equals(object? obj)
        {
            return obj is Name name &&
                   Value == name.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
