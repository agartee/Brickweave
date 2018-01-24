using System.Collections.Generic;
using System.Linq;

namespace Brickweave.Cqrs.Cli.Models
{
    public class ExecutableParameterInfo
    {
        public ExecutableParameterInfo(string name, params string[] values)
        {
            Name = name;
            Values = values ?? new string[0];
        }
        
        public string Name { get; }
        public IEnumerable<string> Values { get; }

        public string SingleValue => Values.FirstOrDefault();

        private bool Equals(ExecutableParameterInfo other)
        {
            return new HashSet<string>(Values).SetEquals(other.Values)
                   && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() 
                && Equals((ExecutableParameterInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 19;
                foreach (var value in Values.OrderBy(t => t))
                    hashCode = hashCode * 31 + value.GetHashCode();

                hashCode = hashCode * 7 + Name.GetHashCode();
                return hashCode;
            }
        }
    }
}