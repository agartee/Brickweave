using System.Collections.Generic;

namespace Brickweave.Cqrs.Cli.Models
{
    public class HelpInfo
    {
        public HelpInfo(string name, string subject, string description, HelpInfoType type, 
            IEnumerable<HelpInfo> children = null)
        {
            Name = name;
            Description = description;
            Type = type;
            Subject = subject;
            Children = children ?? new HelpInfo[0];
        }

        public string Name { get; }
        public string Subject { get; }
        public string Description { get; }
        public HelpInfoType Type { get; }
        public IEnumerable<HelpInfo> Children { get; }

        public static HelpInfo Default()
        {
            return new HelpInfo("root", null, null, HelpInfoType.Category, new HelpInfo[0]);
        }

        public HelpInfo WithAmendedName(string name)
        {
            return new HelpInfo(name, Subject, Description, Type, Children);    
        }

        public HelpInfo WithChildren(IEnumerable<HelpInfo> children)
        {
            return new HelpInfo(Name, Subject, Description, Type, children);
        }

        public override string ToString()
        {
            return $"{nameof(Name)}:{Name};{nameof(Subject)}:{Subject};{nameof(Type)}:{Type};";
        }
    }
}
