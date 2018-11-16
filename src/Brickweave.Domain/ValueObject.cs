namespace Brickweave.Domain
{
    public abstract class ValueObject<T>
    {
        protected ValueObject(T value)
        {
            Value = value;
        }

        public T Value { get; }

        private bool Equals(ValueObject<T> other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is T) return obj.Equals(Value);
            return obj.GetType() == GetType() && Equals((ValueObject<T>)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}