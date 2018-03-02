using System.Collections.Generic;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Tests.Builders
{
    public class HelpInfoBuilder
    {
        private string _name;
        private string _subject;
        private string _description;
        private HelpInfoType _helpInfoType;
        private readonly IList<HelpInfo> _children = new List<HelpInfo>();

        public HelpInfoBuilder WithName(string value)
        {
            _name = value;
            return this;
        }

        public HelpInfoBuilder WithSubject(string value)
        {
            _subject = value;
            return this;
        }

        public HelpInfoBuilder WithDescription(string value)
        {
            _description = value;
            return this;
        }

        public HelpInfoBuilder WithType(HelpInfoType value)
        {
            _helpInfoType = value;
            return this;
        }

        public HelpInfoBuilder WithChild(HelpInfoBuilder builder)
        {
            _children.Add(builder.Build());
            return this;
        }

        public HelpInfo Build()
        {
            return new HelpInfo(_name, _subject, _description, _helpInfoType, _children);
        }
    }
}
