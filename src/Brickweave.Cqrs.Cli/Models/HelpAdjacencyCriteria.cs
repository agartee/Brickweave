namespace Brickweave.Cqrs.Cli.Models
{
    public class HelpAdjacencyCriteria
    {
        public HelpAdjacencyCriteria(string subject, string action = null)
        {
            Subject = subject ?? string.Empty;
            Action = action ?? string.Empty;
        }
        
        public string Subject { get; }
        public string Action { get; }

        public static HelpAdjacencyCriteria Empty()
        {
            return new HelpAdjacencyCriteria(null);
        }
        
        private bool Equals(HelpAdjacencyCriteria other)
        {
            return string.Equals(Subject, other.Subject) 
                && string.Equals(Action, other.Action);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() 
                && Equals((HelpAdjacencyCriteria) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Subject != null ? Subject.GetHashCode() : 0) * 397) ^ (Action != null ? Action.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return Subject;
        }
    }
}
